using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace FormerUrban_Afta.Attributes;
public class CustomClaimsPrincipalFactory : UserClaimsPrincipalFactory<CostumIdentityUser, CostumIdentityRole>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CustomClaimsPrincipalFactory(
        UserManager<CostumIdentityUser> userManager,
        RoleManager<CostumIdentityRole> roleManager,
        IOptions<IdentityOptions> optionsAccessor,
        IHttpContextAccessor httpContextAccessor)
        : base(userManager, roleManager, optionsAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public override async Task<ClaimsPrincipal> CreateAsync(CostumIdentityUser user)
    {
        var principal = await base.CreateAsync(user);
        var identity = (ClaimsIdentity)principal.Identity!;

        if (_httpContextAccessor.HttpContext?.Items["SessionId"] is string sessionId)
        {
            identity.AddClaim(new Claim("SessionId", sessionId));
        }

        return principal;
    }
}
