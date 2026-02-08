using FormerUrban_Afta.DataAccess.DTOs.IdentityUser;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FormerUrban_Afta.DataAccess.Services.Interfaces;
public interface IPermissionService
{
    public IReadOnlyList<UserDto> GetUsersByRole(string role);
    public Task<bool> HasPermissionAsync(string userId, string permissionName);
    public Task CheckHash(string userId, AuthorizationFilterContext context);
}