using DNTPersianUtils.Core;
using FormerUrban_Afta.DataAccess.DTOs.Login;
using Microsoft.Extensions.DependencyInjection;

namespace FormerUrban_Afta.DataAccess.Services;
class UserSessionService : IUserSessionService
{
    private readonly FromUrbanDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IServiceProvider _serviceProvider;
    private readonly IBrowserService _browserService;
    private readonly IIpService _ipService;

    public UserSessionService(FromUrbanDbContext context, IHttpContextAccessor httpContextAccessor, IServiceProvider serviceProvider, IBrowserService browserService, IIpService ipService)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        _serviceProvider = serviceProvider;
        _browserService = browserService;
        _ipService = ipService;
    }

    #region Shit

    public static bool CheckHash(UserSession obj)
    {
        var hash = CipherService.IsEqual(obj.ToString(), obj.Hashed);
        return hash;
    }

    #endregion


    public async Task<List<UserSessionDto>> GetAllAsync()
    {
        var users = await _context.Users.AsNoTracking().ToListAsync();
        var userDict = users.ToDictionary(u => u.Id, u => u);
        var sessions = await _context.UserSession.AsNoTracking().ToListAsync();

        var result = sessions.Select(x =>
        {
            userDict.TryGetValue(x.UserId, out var user);
            return new UserSessionDto
            {
                Ip = x.Ip,
                FullName = user?.Name + " " + user?.Family,
                CreatedAt = x.CreatedAt.ToPersianDateTimeString("HH:mm:ss yyyy/MM/dd", true),
                LastActivity = DateTime.SpecifyKind((DateTime)x.LastActivity, DateTimeKind.Local).ToPersianDateTimeString("HH:mm:ss yyyy/MM/dd", true),
                ExpiresAt = DateTime.SpecifyKind((DateTime)x.ExpiresAt, DateTimeKind.Local).ToPersianDateTimeString("HH:mm:ss yyyy/MM/dd", true),
                UserAgent = x.UserAgent,
                IsValid = CheckHash(x),
                Identity = x.Id
            };
        }).OrderByDescending(x => x.CreatedAt).ToList();

        return result;
    }

    public async Task<List<UserSessionDto>> GetByUserAsync(string UserId)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == UserId);
        var sessions = await _context.UserSession.Where(x => x.UserId == UserId).AsNoTracking().ToListAsync();

        return sessions.Select(x => new UserSessionDto
        {
            Ip = x.Ip,
            FullName = user?.Name + " " + user?.Family,
            CreatedAt = x.CreatedAt.ToPersianDateTimeString("HH:mm:ss yyyy/MM/dd", true),
            LastActivity = DateTime.SpecifyKind((DateTime)x.LastActivity, DateTimeKind.Local).ToPersianDateTimeString("HH:mm:ss yyyy/MM/dd", true),
            ExpiresAt = DateTime.SpecifyKind((DateTime)x.ExpiresAt, DateTimeKind.Local).ToPersianDateTimeString("HH:mm:ss yyyy/MM/dd", true),
            UserAgent = x.UserAgent,
            IsValid = CheckHash(x),
            Identity = x.Id
        }).OrderByDescending(x => x.CreatedAt).ToList();
    }

    public async Task<AuthResponse> Delete(Guid id)
    {
        var response = new AuthResponse();
        try
        {
            var data = await _context.UserSession.FirstOrDefaultAsync(x => x.Id == id);
            if (data == null)
                return response.IsFailed("رکورد مورد نظر یافت نشد");
            var userId = data.UserId;
            _context.UserSession.Remove(data);
            var res = await _context.SaveChangesAsync();
            var authService = _serviceProvider.GetRequiredService<IAuthService>();
            var userData = await authService.GetByUserNameAsNoTrackingAsync(userId);
            return res > 0 ? response.IsSuccess(userName: userData?.UserName ?? string.Empty) :
                response.IsFailed("خطایی در حذف نشست کاربر رخ داده!", userName: userData?.UserName ?? string.Empty);
        }
        catch (Exception e)
        {
            return response.IsFailed(e.Message);
        }
    }

    public async Task AddNew(CostumIdentityUser userModel)
    {
        var tarifhaService = _serviceProvider.GetRequiredService<ITarifhaService>();
        var tarifha = await tarifhaService.GetTarifhaNoLogAsync();

        var sessionId = Guid.NewGuid().ToString();

        // ⬅️ Store in HttpContext.Items so the CustomClaimsPrincipalFactory can pick it up
        _httpContextAccessor.HttpContext!.Items["SessionId"] = sessionId;

        var now = DateTime.UtcNow.AddHours(3.5);
        if (string.IsNullOrWhiteSpace(tarifha?.KhatemeSessionAfterMinute))
            tarifha.KhatemeSessionAfterMinute = "5";

        var sessionLifetime = TimeSpan.FromMinutes(Convert.ToInt32(tarifha?.KhatemeSessionAfterMinute));

        // 🗃 Save the session to DB
        _context.UserSession.Add(new UserSession
        {
            UserId = userModel.Id,
            SessionId = sessionId,
            CreatedAt = now,
            LastActivity = now,
            ExpiresAt = now.Add(sessionLifetime),
            UserAgent = _browserService.GetBrowserDetails(),
            Ip = _ipService.GetIp(),
        });
        await _context.SaveChangesAsync();
    }

    public async Task<bool> CheckUserMaxActiveSessions(CostumIdentityUser userModel)
    {
        var tarifhaService = _serviceProvider.GetRequiredService<ITarifhaService>();
        var tarifha = await tarifhaService.GetTarifhaNoLogAsync();

        var activeSessions = await _context.UserSession
            .Where(s => s.UserId == userModel.Id)
            .OrderBy(s => s.CreatedAt)
            .ToListAsync();

        if (string.IsNullOrWhiteSpace(tarifha?.MaximumSessions))
            tarifha.MaximumSessions = "3";

        var maxSessionsPerUser = Convert.ToInt32(tarifha?.MaximumSessions);
        var idleSessions = activeSessions.Where(c => c.ExpiresAt < (DateTime.UtcNow.AddHours(3.5))).ToList();

        if (idleSessions.Count > 0)
        {
            foreach (var session in idleSessions)
            {
                await SessionExpiratin(session);
            }

            await SaveChangesAsync();
        }

        if (activeSessions.Count - idleSessions.Count >= maxSessionsPerUser)
        {
            // Prevent new login
            return false;
        }

        return true;
    }

    private async Task<bool> SessionExpiratin(UserSession session)
    {
        _context.UserSession.Remove(session);
        return true;
    }

    private async Task<bool> SessionExpiratin(List<UserSession> session)
    {
        _context.UserSession.RemoveRange(session);
        return true;
    }

    public async Task<bool> UserSessionDeactivate(string userId)
    {
        var sessionId = _httpContextAccessor.HttpContext?.User.FindFirst("SessionId")?.Value;
        if (string.IsNullOrEmpty(sessionId))
            return false;

        var obj = await _context.UserSession.FirstOrDefaultAsync(c =>
             c.UserId == userId && c.SessionId == sessionId);
        if (obj == null)
            return false;

        await SessionExpiratin(obj); // expires the session
        var res = await SaveChangesAsync();
        return res;
    }

    public async Task<bool> UserSessionDeactivate2(string userId)
    {
        var obj = await _context.UserSession.Where(c => c.UserId == userId).ToListAsync();
        if (!obj.Any())
            return false;

        await SessionExpiratin(obj); // expires the session
        var res = await SaveChangesAsync();
        return res;
    }

    private async Task<bool> SaveChangesAsync()
    {
        var res = await _context.SaveChangesAsync();
        return res > 0;
    }
}
