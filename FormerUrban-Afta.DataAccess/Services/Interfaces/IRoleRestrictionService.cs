using FormerUrban_Afta.DataAccess.DTOs.Login;

namespace FormerUrban_Afta.DataAccess.Services.Interfaces;
public interface IRoleRestrictionService
{
    public Task<List<RoleRestrictionDto>> GetAllActiveRows();
    public Task<List<RoleRestrictionDto>> GetAll();
    public Task<RoleRestrictionDto> GetById(long id);
    public Task<RoleRestrictionDto> GetByIdAsNoTracking(long id);
    public Task<RoleRestrictionDto> GetByRoleIdActive(string roleId);
    public Task<AuthResponse> Add(RoleRestrictionDto roleRestrictionDto);
    public Task<AuthResponse> Delete(long id);
    public Task<AuthResponse> Update(RoleRestrictionDto roleRestrictionDto);
    public Task<bool> IsUserRestricted(string userId);
}
