using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace FormerUrban_Afta.Attributes;
public class OptimizedSecurityStampValidator : SecurityStampValidator<CostumIdentityUser>
{
    private readonly IMemoryCache _cache;
    private readonly ILogger<OptimizedSecurityStampValidator> _logger;
    private static readonly TimeSpan CacheDuration = TimeSpan.FromSeconds(1);
    private readonly IUserSessionService _userSessionService;
    private readonly IUserLoginedService _userLoginedService;
    private readonly IBrowserService _browserService;
    private readonly IIpService _ipService;

    public OptimizedSecurityStampValidator(IOptions<SecurityStampValidatorOptions> options, SignInManager<CostumIdentityUser> signInManager,
        ILoggerFactory loggerFactory, IMemoryCache cache, ILogger<OptimizedSecurityStampValidator> logger, IUserSessionService userSessionService,
        IUserLoginedService userLoginedService, IBrowserService browserService, IIpService ipService) : base(options, signInManager, loggerFactory)
    {
        _cache = cache;
        _logger = logger;
        _userSessionService = userSessionService;
        _userLoginedService = userLoginedService;
        _browserService = browserService;
        _ipService = ipService;
    }

    public override async Task ValidateAsync(CookieValidatePrincipalContext context)
    {
        var userId = context.Principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            _logger.LogWarning("No user ID found in claims");
            await base.ValidateAsync(context);
            return;
        }

        var cacheKey = $"SecurityStamp_{userId}";

        if (_cache.TryGetValue(cacheKey, out _))
        {
            return;
        }


        try
        {
            var user = await SignInManager.UserManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("User not found in database");
                context.RejectPrincipal();
                await SignInManager.SignOutAsync();
                return;
            }
            //context.Principal.FindFirstValue()
            var cookieStamp = context.Principal.FindFirst("AspNet.Identity.SecurityStamp")?.Value;

            if (cookieStamp != user.SecurityStamp)
            {
                _logger.LogWarning("Security stamp mismatch");
                await _userSessionService.UserSessionDeactivate2(userId);
                var badLogin = new UserLogined
                {
                    UserName = user?.UserName ?? "",
                    FullName = $"{user?.Name} {user?.Family}",
                    LogoutDatetime = DateTime.UtcNow.AddHours(3.5),
                    LoginDateTime = null,
                    UserCode = user?.Id ?? "",
                    UserAgent = _browserService.GetBrowserDetails(),
                    Ip = _ipService.GetIp(),
                    Method = "خروج کاربر به علت ویرایش اطلاعات کاربری",
                    Status = (byte)UserLoginStatus.Logout
                };
                _userLoginedService.Insert(badLogin);
                context.RejectPrincipal();
                await SignInManager.SignOutAsync();
            }
            else
            {
                _cache.Set(cacheKey, true, CacheDuration);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during security stamp validation");
            throw;
        }
    }
}