using FormerUrban_Afta.DataAccess.DTOs.IdentityUser;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Data;

namespace FormerUrban_Afta.DataAccess.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly UserManager<CostumIdentityUser> _userManager;
        private readonly FromUrbanDbContext _context;

        public PermissionService(UserManager<CostumIdentityUser> userManager, FromUrbanDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<bool> HasPermissionAsync(string userId, string permissionName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            var roles = await _userManager.GetRolesAsync(user);
            var permissionId = (int)(EnumPermission)Enum.Parse(typeof(EnumPermission), permissionName);

            try
            {
                var rolePermissions = await _context.RolePermission.AnyAsync(rp => rp.Role.Name != null && roles.Contains(rp.Role.Name) && rp.PermissionId == permissionId);

                var userPermissions = await _context.UserPermission.AnyAsync(up => up.UserId == userId && up.PermissionId == permissionId);

                return rolePermissions || userPermissions;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task CheckHash(string userId, AuthorizationFilterContext context)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return;
            if (!user.LockoutEnabled)
                context.Result = new RedirectToActionResult("Logout", "Login", new { area = "", userDeactive = true });

            bool invalidHash = false;
            if (!CipherService.IsEqual(user.ToString(), user.Hashed))
                context.Result = new RedirectToActionResult("Logout", "Login", new { area = "", invalidHashUser = true });

            var userRoles = await _context.UserRoles.Where(a => a.UserId == user.Id).ToListAsync();

            if (userRoles.Any())
            {
                foreach (var role in userRoles)
                {
                    if (invalidHash)
                        break;

                    if (!CipherService.IsEqual(role.ToString(), role.Hashed))
                    {
                        invalidHash = true;
                        break;
                    }
                    var rolePermissions = await _context.RolePermission.Where(rp => rp.RoleId == role.RoleId).ToListAsync();

                    foreach (var permission in rolePermissions)
                    {
                        if (!CipherService.IsEqual(permission.ToString(), permission.Hashed))
                        {
                            invalidHash = true;
                            break;
                        }
                    }
                }
            }

            if (!invalidHash)
            {
                var userPermissions = await _context.UserPermission.Where(up => up.UserId == user.Id).ToListAsync();
                foreach (var item in userPermissions)
                {
                    if (!CipherService.IsEqual(item.ToString(), item.Hashed))
                    {
                        invalidHash = true;
                        break;
                    }
                }
            }

            if (invalidHash)
                context.Result = new RedirectToActionResult("Logout", "Login", new { area = "", invalidHashRole = true });
        }

        #region Role User
        public IReadOnlyList<UserDto> GetUsersByRole(string role)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(role))
                {
                    throw new ArgumentException("Role name cannot be null or empty", nameof(role));
                }

                var users = _context.Users
                    .Join(_context.UserRoles,
                        u => u.Id,
                        ur => ur.UserId,
                        (u, ur) => new { User = u, ur.RoleId })
                    .Join(_context.Roles.Where(r => r.Name == role),
                        ur => ur.RoleId,
                        r => r.Id,
                        (ur, r) => new UserDto
                        {
                            Id = ur.User.Id,
                            Name = ur.User.Name,
                            Family = ur.User.Family
                        })
                    .ToList();

                return users.AsReadOnly();
            }
            catch (Exception ex)
            {
                // Log exception (implement your logging mechanism)
                throw;
            }
        }
        #endregion
    }

}
