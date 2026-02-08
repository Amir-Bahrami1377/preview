using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;

namespace FormerUrban_Afta.Middlewares;

public class BotDetectionMiddleware : IMiddleware
{
    private readonly IMemoryCache _cache;
    private readonly ILogger<BotDetectionMiddleware> _logger;
    private readonly BotDetectionSettings _settings;

    public BotDetectionMiddleware(IMemoryCache cache, ILogger<BotDetectionMiddleware> logger, IOptions<BotDetectionSettings> settings)
    {
        _cache = cache;
        _logger = logger;
        _settings = settings.Value;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var path = context.Request.Path;
        var method = context.Request.Method;
        var ip = GetClientIp(context);

        if (IsBypassPath(path))
        {
            await next(context);
            return;
        }

        if (context.Request.Cookies.TryGetValue("BotVerified", out var verified) && verified == "true")
        {
            await next(context);
            return;
        }

        if (_cache.TryGetValue(GetBotKey(ip), out _))
        {
            await DenyAccess(context, ip, "Previously flagged bot");
            return;
        }

        int botScore = CalculateBotScore(context);

        if (botScore >= _settings.BotScoreThreshold)
        {
            await DenyAccess(context, ip, $"Bot score exceeded threshold ({botScore})");
            return;
        }

        if (_settings.EnableJsChallenge && method == HttpMethods.Get)
        {
            if (!ValidateJsTokenFromCookie(context, ip))
            {
                await IssueJsChallenge(context, ip);
                return;
            }
        }

        if (method == HttpMethods.Post && context.Request.HasFormContentType)
        {
            if (await HandleJsPostChallenge(context, ip, next))
                return;
        }

        await next(context);
    }

    private string GetClientIp(HttpContext context)
    {
        return context.Request.Headers["X-Forwarded-For"].FirstOrDefault() ?? context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
    }

    private bool IsBypassPath(PathString path)
    {
        return path.StartsWithSegments("/BotChallenge") || path.Value?.Contains('.') == true;
    }

    private int CalculateBotScore(HttpContext context)
    {
        int score = 0;
        string ua = context.Request.Headers["User-Agent"].ToString();
        if (string.IsNullOrEmpty(ua)) score += 40;
        else if (_settings.SuspiciousUserAgents.Any(s => ua.Contains(s, StringComparison.OrdinalIgnoreCase))) score += 30;

        if (!context.Request.Headers.ContainsKey("Accept") || !context.Request.Headers.ContainsKey("Accept-Language"))
            score += 20;

        string ip = GetClientIp(context);
        string rateKey = $"BotRate_{ip}";

        if (_cache.TryGetValue(rateKey, out int count))
        {
            if (count >= _settings.MaxRequestsPerMinute)
                score += 30;
            _cache.Set(rateKey, count + 1, TimeSpan.FromMinutes(1));
        }
        else
        {
            _cache.Set(rateKey, 1, TimeSpan.FromMinutes(1));
        }

        return score;
    }

    private async Task<bool> HandleJsPostChallenge(HttpContext context, string ip, RequestDelegate next)
    {
        var form = await context.Request.ReadFormAsync();
        string posted = form["jsToken"];
        string expectedKey = GetTokenKey(ip);
        string nonceKey = GetNonceKey(ip);

        if (_cache.TryGetValue(expectedKey, out string expected) &&
            _cache.TryGetValue(nonceKey, out string nonce) &&
            ValidateJsToken(posted, expected, nonce))
        {
            _logger.LogInformation("Client {IP} passed JS challenge", ip);
            _cache.Remove(expectedKey);
            _cache.Remove(nonceKey);

            context.Response.Cookies.Append("BotVerified", "true", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            });

            await next(context);
            return true;
        }

        _logger.LogWarning("Invalid JS token from {IP}. Posted: {Posted}, DecodedPosted: {DecodedPosted}, Expected: {Expected}, DecodedExpected: {DecodedExpected}",
            ip, posted, DecodeBase64(posted), expected, DecodeBase64(expected));

        await DenyAccess(context, ip, "Invalid JS token");
        return true;
    }

    private async Task DenyAccess(HttpContext context, string ip, string reason)
    {
        _logger.LogWarning("Access denied for {IP}. Reason: {Reason}", ip, reason);
        _cache.Set(GetBotKey(ip), 1, TimeSpan.FromMinutes(1));
        context.Response.StatusCode = 403;
        await context.Response.WriteAsync("Access denied: Bot activity detected.");
    }

    private async Task IssueJsChallenge(HttpContext context, string ip)
    {
        string nonce = context.Items["CSP-Nonce"]?.ToString();
        if (string.IsNullOrEmpty(nonce))
        {
            _logger.LogWarning("Missing CSP nonce.");
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync("Internal server error.");
            return;
        }

        string token = GenerateTokenWithNonce(nonce);
        _cache.Set(GetTokenKey(ip), token, TimeSpan.FromMinutes(2));
        _cache.Set(GetNonceKey(ip), nonce, TimeSpan.FromMinutes(2));

        context.Response.Cookies.Append("JsToken", token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict
        });

        context.Response.Headers["Content-Security-Policy"] = $"script-src 'nonce-{nonce}'; object-src 'none'; base-uri 'none';";
        context.Response.ContentType = "text/html";
        await context.Response.WriteAsync($"""
            <html>
            <body>
                <form id='challenge' method='post' action='{context.Request.Path}'>
                    <input type='hidden' name='jsToken' value='{token}' />
                </form>
                <script nonce='{nonce}'>
                    document.getElementById('challenge').submit();
                </script>
            </body>
            </html>
        """);
    }

    private bool ValidateJsToken(string received, string expected, string nonce)
    {
        if (!CryptographicOperations.FixedTimeEquals(Convert.FromBase64String(received), Convert.FromBase64String(expected)))
            return false;

        var decoded = DecodeBase64(received);
        var parts = decoded.Split(':');
        return parts.Length == 2 && parts[1] == nonce;
    }

    private bool ValidateJsTokenFromCookie(HttpContext context, string ip)
    {
        if (!context.Request.Cookies.TryGetValue("JsToken", out string jsToken))
            return false;

        if (!_cache.TryGetValue(GetTokenKey(ip), out string storedToken))
            return false;

        if (!_cache.TryGetValue(GetNonceKey(ip), out string storedNonce))
            return false;

        return ValidateJsToken(jsToken, storedToken, storedNonce);
    }

    private string GenerateTokenWithNonce(string nonce)
    {
        var raw = $"{Guid.NewGuid()}:{nonce}";
        return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(raw));
    }

    private string DecodeBase64(string b64)
    {
        try
        {
            return System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(b64));
        }
        catch
        {
            return "<invalid base64>";
        }
    }

    private string GetBotKey(string ip) => $"botKey_{ip}";
    private string GetTokenKey(string ip) => $"JsToken_{ip}";
    private string GetNonceKey(string ip) => $"Nonce_{ip}";
}

public class BotDetectionSettings
{
    public int MaxRequestsPerMinute { get; set; } = 10;
    public int BotScoreThreshold { get; set; } = 70;
    public bool EnableJsChallenge { get; set; } = true;
    public List<string> SuspiciousUserAgents { get; set; } = new() { "bot", "crawler", "spider", "curl", "wget" };
}
