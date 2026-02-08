using FormerUrban_Afta.DataAccess.DTOs.Login;

namespace FormerUrban_Afta.DataAccess.Services.Interfaces;
public interface IUserSessionService
{
    public Task AddNew(CostumIdentityUser userModel);
    public Task<bool> CheckUserMaxActiveSessions(CostumIdentityUser userModel);
    public Task<bool> UserSessionDeactivate(string userId);
    public Task<List<UserSessionDto>> GetAllAsync();
    public Task<List<UserSessionDto>> GetByUserAsync(string UserId);
    public Task<AuthResponse> Delete(Guid id);
    public Task<bool> UserSessionDeactivate2(string userId);
}
