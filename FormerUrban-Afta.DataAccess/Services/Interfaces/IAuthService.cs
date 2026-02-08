
using FormerUrban_Afta.DataAccess.DTOs.Login;

namespace FormerUrban_Afta.DataAccess.Services.Interfaces;
public interface IAuthService
{
    Task<AuthResponse> CheckLogin(AuthRequest request);
    Task<AuthResponse> SmsLogin(AuthRequest request);
    Task<IReadOnlyList<CostumIdentityUser>> GetAllAsync();
    Task<CostumIdentityUser> AddAsync(CostumIdentityUser entity);
    Task<AuthResponse> OtpLogin(AuthRequest request);
    Task<CostumIdentityUser> GetCurentUserAsync();
    CostumIdentityUser GetCurrentUser();
    Task<CostumIdentityUser> GetByUserNameAsync(string userName);
    Task<CostumIdentityUser> GetByUserNameAsNoTrackingAsync(string userName);
    Task<CostumIdentityUser> GetAsync(string id);
    public Task<List<string>> GetRoleByUserIdAsync(string userId);
    Task<IReadOnlyList<CostumIdentityRole>> GetAllRoleAsync();
    Task<CostumIdentityRole> GetRoleByNameAsync(string name);
    Task<IdentityResult> CreateAsync(CostumIdentityUser entity);
    Task<IdentityResult> UpdateAsync(CostumIdentityUser entity);
    Task<bool> AddToRoleAsync(CostumIdentityUser user, string roleName);
    Task<AuthResponse> CheckMobile(string mobile);
    public Task Logout(string userId);
    public Task<bool> CheckPassword(CostumIdentityUser user, string password);
    public Task<bool> BlockUser(string userName);
    public Task<bool> BlockUserByUserId(string userId);
    public Task<AuthResponse> UnBlockUser(string userName);
    public Task<bool> CheckUserReachedMaxLogins(CostumIdentityUser user);
    public Task CreateDeveloperUser();
    public Task SeedRolesAsync();
}