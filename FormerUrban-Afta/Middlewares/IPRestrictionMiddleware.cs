using Microsoft.Extensions.Caching.Memory;
using System.Net;

namespace FormerUrban_Afta.Middlewares;
public class IPRestrictionMiddleware : IMiddleware
{
    private readonly ILogger<IPRestrictionMiddleware> _logger;
    private readonly FromUrbanDbContext _dbContext;
    private readonly IMemoryCache _cache;

    public IPRestrictionMiddleware(
        ILogger<IPRestrictionMiddleware> logger,
        FromUrbanDbContext dbContext, IMemoryCache cache)
    {
        _logger = logger;
        _dbContext = dbContext;
        _cache = cache;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var remoteIp = context.Connection.RemoteIpAddress;

        if (remoteIp == null)
        {
            _logger.LogWarning("آیپی کاربر یافت نشد");
            context.Response.StatusCode = StatusCodes.Status403Forbidden;

            return;
        }

        string clientIp = remoteIp.ToString();

        // Step 1: Check whitelist
        bool hasWhitelist = await HasActiveWhitelist();
        if (hasWhitelist)
        {
            bool isWhitelisted = await IsIpWhitelisted(clientIp);
            if (!isWhitelisted)
            {
                _logger.LogWarning($"دسترسی کاربر با آیپی {clientIp} رد شد - فقط آیپی‌های لیست سفید مجازند", clientIp);
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Access Denied");
                return;
            }
        }
        else
        {
            // Step 2: If no whitelist, check blacklist
            bool isBlocked = await IsIpBlocked(clientIp);
            if (isBlocked)
            {
                _logger.LogWarning("قطع دسترسی کاربر با آیپی: {ClientIp}", clientIp);
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Access Denied");
                return;
            }
        }

        await next(context);
    }

    private async Task<bool> HasActiveWhitelist()
    {
        var currentDate = DateTime.UtcNow.AddHours(3.5);
        return await _dbContext.AllowedIPRange
            .AsNoTracking()
            .AnyAsync(r => (r.FromDate == null || r.FromDate <= currentDate) &&
                           (r.ToDate == null || r.ToDate >= currentDate));
    }

    private async Task<bool> IsIpWhitelisted(string clientIp)
    {
        var currentDate = DateTime.UtcNow.AddHours(3.5);
        var allowedRanges = await _dbContext.AllowedIPRange
            .AsNoTracking()
            .Where(r => (r.FromDate == null || r.FromDate <= currentDate) &&
                        (r.ToDate == null || r.ToDate >= currentDate))
            .Select(r => r.IPRange)
            .ToListAsync();

        foreach (var range in allowedRanges)
        {
            if (IsIpInRange(clientIp, range))
            {
                return true;
            }
        }
        return false;
    }

    private async Task<bool> IsIpBlocked(string clientIp)
    {
        var blockIp = $"blockIp_{clientIp}";
        if (_cache.TryGetValue(blockIp, out int isBlocked))
        {
            _cache.Set(blockIp, 1, TimeSpan.FromMinutes(5));
            return true;
        }
        var currentDate = DateTime.UtcNow.AddHours(3.5);

        var blockedRanges = await _dbContext.BlockedIPRange
            .AsNoTracking()
            .Where(r => (r.FromDate == null || r.FromDate <= currentDate) &&
                        (r.ToDate == null || r.ToDate >= currentDate))
            .Select(r => r.IPRange)
            .ToListAsync();

        foreach (var range in blockedRanges)
        {
            if (IsIpInRange(clientIp, range))
            {
                _cache.Set(blockIp, 1, TimeSpan.FromMinutes(5));
                return true;
            }
        }
        return false;
    }

    private bool IsIpInRange(string ip, string range)
    {
        try
        {
            if (range.Contains('/'))
            {
                var ipNetwork = IPNetwork2.Parse(range);
                var clientIpAddress = IPAddress.Parse(ip);
                return ipNetwork.Contains(clientIpAddress);
            }
            else if (range.Contains('-'))
            {
                var parts = range.Split('-');
                var startIp = IPAddress.Parse(parts[0].Trim());
                var endIp = IPAddress.Parse(parts[1].Trim());
                var clientIpAddress = IPAddress.Parse(ip);

                byte[] startBytes = startIp.GetAddressBytes();
                byte[] endBytes = endIp.GetAddressBytes();
                byte[] clientBytes = clientIpAddress.GetAddressBytes();

                return CompareIp(startBytes, clientBytes) <= 0 &&
                       CompareIp(clientBytes, endBytes) <= 0;
            }
            else
            {
                return ip == range;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking IP range: {Range}", range);
            return false;
        }
    }

    private int CompareIp(byte[] ip1, byte[] ip2)
    {
        for (int i = 0; i < ip1.Length; i++)
        {
            if (ip1[i] != ip2[i])
                return ip1[i] - ip2[i];
        }
        return 0;
    }

}

