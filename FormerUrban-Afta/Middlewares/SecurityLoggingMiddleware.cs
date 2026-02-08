namespace FormerUrban_Afta.Middlewares;

public class SecurityLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<SecurityLoggingMiddleware> _logger;

    public SecurityLoggingMiddleware(RequestDelegate next, ILogger<SecurityLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Log potential security threats
        await LogSecurityThreats(context);

        await _next(context);

        // Log response security information
        await LogSecurityResponse(context);
    }

    private async Task LogSecurityThreats(HttpContext context)
    {
        var request = context.Request;
        var clientIP = context.Connection.RemoteIpAddress?.ToString();
        var userAgent = request.Headers["User-Agent"].FirstOrDefault();

        // Detect potential SQL injection
        if (ContainsSqlInjectionPatterns(request.QueryString.Value))
        {
            _logger.LogWarning("Potential SQL injection detected from IP {ClientIP}. Query: {QueryString}",
                clientIP, request.QueryString.Value);
        }

        // Detect potential XSS
        if (ContainsXssPatterns(request.QueryString.Value))
        {
            _logger.LogWarning("Potential XSS attack detected from IP {ClientIP}. Query: {QueryString}",
                clientIP, request.QueryString.Value);
        }

        // Log suspicious user agents
        if (IsSuspiciousUserAgent(userAgent))
        {
            _logger.LogWarning("Suspicious user agent detected from IP {ClientIP}. UserAgent: {UserAgent}",
                clientIP, userAgent);
        }

        // Log file upload attempts to unauthorized paths
        if (request.Path.StartsWithSegments("/upload") && !context.User.Identity.IsAuthenticated)
        {
            _logger.LogWarning("Unauthorized file upload attempt from IP {ClientIP} to path {Path}",
                clientIP, request.Path);
        }
    }

    private async Task LogSecurityResponse(HttpContext context)
    {
        // Log 4xx and 5xx responses for security analysis
        if (context.Response.StatusCode >= 400)
        {
            _logger.LogWarning("HTTP {StatusCode} response sent to IP {ClientIP} for path {Path}",
                context.Response.StatusCode,
                context.Connection.RemoteIpAddress?.ToString(),
                context.Request.Path);
        }
    }

    private bool ContainsSqlInjectionPatterns(string input)
    {
        if (string.IsNullOrEmpty(input)) return false;

        var patterns = new[] { "union", "select", "insert", "delete", "drop", "exec", "script" };
        return patterns.Any(pattern => input.ToLower().Contains(pattern));
    }

    private bool ContainsXssPatterns(string input)
    {
        if (string.IsNullOrEmpty(input)) return false;

        var patterns = new[] { "<script", "javascript:", "onerror=", "onload=" };
        return patterns.Any(pattern => input.ToLower().Contains(pattern));
    }

    private bool IsSuspiciousUserAgent(string userAgent)
    {
        if (string.IsNullOrEmpty(userAgent)) return true;

        var suspiciousPatterns = new[] { "sqlmap", "nikto", "nmap", "masscan" };
        return suspiciousPatterns.Any(pattern => userAgent.ToLower().Contains(pattern));
    }
}

