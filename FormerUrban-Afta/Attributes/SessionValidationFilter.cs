namespace FormerUrban_Afta.Attributes;
public class SessionValidationFilter : IAsyncActionFilter
{
    private readonly FromUrbanDbContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ITarifhaService _tarifhaService;

    // Define exclusions here
    private static readonly string[] SkipPaths = ["/login", "/logout", "/heartbeat", "/home/exceptionerror"];

    public SessionValidationFilter(
        FromUrbanDbContext dbContext,
        IHttpContextAccessor httpContextAccessor,
        ITarifhaService tarifhaService)
    {
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
        _tarifhaService = tarifhaService;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var httpContext = _httpContextAccessor.HttpContext!;
        var user = httpContext.User;

        // 1. Skip anonymous users
        if (user?.Identity?.IsAuthenticated != true)
        {
            await next();
            return;
        }

        // 2. Skip if in excluded paths (like /login)
        var requestPath = httpContext.Request.Path.ToString().ToLower();
        if (SkipPaths.Any(path => requestPath.StartsWith(path)))
        {
            await next();
            return;
        }

        // 4. Validate session
        var sessionId = user.FindFirst("SessionId")?.Value;
        var userId = user.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
        if (string.IsNullOrEmpty(sessionId))
        {
            context.Result = new RedirectToActionResult("Logout", "Login", new { area = "", sessionExpired = true });
            return;
        }
        var session = await _dbContext.UserSession.FirstOrDefaultAsync(s => s.SessionId == sessionId && s.UserId == userId);

        if (session == null || session.ExpiresAt < DateTime.UtcNow.AddHours(3.5))
        {
            //context.Result = new RedirectToActionResult("Logout", "Login", new { area = "", sessionExpired = true });
            HandleSessionExpired(context, httpContext);
            return;
        }

        var checkHash = CipherService.IsEqual(session.ToString(), session.Hashed);
        if (!checkHash)
        {
            context.Result = new RedirectToActionResult("Logout", "Login", new { area = "", hashNotValid = true });
            return;
        }

        var tarifha = await _tarifhaService.GetTarifhaNoLogAsync();
        var sessionLifetime = TimeSpan.FromMinutes(5);

        if (tarifha is { KhatemeSessionAfterMinute: not null })
            sessionLifetime = TimeSpan.FromMinutes(Convert.ToInt32(tarifha.KhatemeSessionAfterMinute));

        // Update last activity
        var now = DateTime.UtcNow.AddHours(3.5);
        session.LastActivity = now;
        session.ExpiresAt = now.Add(sessionLifetime);
        var res = await _dbContext.SaveChangesAsync();

        await next();
    }

    private void HandleSessionExpired(ActionExecutingContext context, HttpContext httpContext)
    {
        var isAjax = httpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
        if (isAjax)
        {
            if (context.Controller is Controller controller)
                controller.TempData["sessionExpired"] = "Expired";

            context.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized);
        }
        else
        {
            context.Result = new RedirectToActionResult("Logout", "Login", new { area = "", sessionExpired = true });
        }
    }
}
