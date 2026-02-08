
namespace FormerUrban_Afta.Attributes;
public class RoleRestrictionFilter : IAsyncActionFilter
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IRoleRestrictionService _roleRestrictionService;
    // Define exclusions here
    private static readonly string[] SkipPaths = ["/login", "/logout", "/heartbeat", "/home/exceptionerror", "/error"];
    public RoleRestrictionFilter(IRoleRestrictionService roleRestrictionService, IHttpContextAccessor httpContextAccessor)
    {
        _roleRestrictionService = roleRestrictionService;
        _httpContextAccessor = httpContextAccessor;
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

        var userId = user.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
        var res = await _roleRestrictionService.IsUserRestricted(userId);

        if (res)
        {
            context.Result = new RedirectToActionResult("Logout", "Login", new { area = "", roleRestriction = true });
            return;
        }

        await next();
    }
}

