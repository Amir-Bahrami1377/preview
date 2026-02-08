using FormerUrban_Afta.DataAccess.DTOs.IdentityUser;

namespace FormerUrban_Afta.DataAccess.Services.Interfaces;
public interface IRolePermissionService : IGenericRepository<RolePermission>
{
    Task<bool> DeleteByRoleIdAsync(string roleId);
    Task<List<RolePermission>> GetAllByRoleIdAsync(string roleId);
    Task<List<RolePermissionDto>> CheckChanges(CostumIdentityUser currentUser, CostumIdentityRole role, List<RolePermissionDto> permissions);
}