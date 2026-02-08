using FormerUrban_Afta.DataAccess.DTOs.IdentityUser;

namespace FormerUrban_Afta.DataAccess.Services.Interfaces
{
    public interface IUserPermissionService : IGenericRepository<UserPermission>
    {
        Task<bool> DeleteByUserIdAsync(string userId);
        Task<List<UserPermission>> GetAllByUserIdAsync(string userId);
        Task<List<UserPermission>> GetAllByUserIdAsync(CostumIdentityUser user);
        Task CheckChanges(CostumIdentityUser currentUser, CostumIdentityUser user, List<UserPermissionDto> permissions);
        Task DeleteByRoleName(CostumIdentityUser user, string[] roleNames);
        public Task<RolePermission> GetRoleAsync(long id);
    }
}
