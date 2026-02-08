

using FormerUrban_Afta.DataAccess.DTOs.IdentityUser;
using System.Data;

namespace FormerUrban_Afta.DataAccess.Services
{
    public class UserPermissionService : IUserPermissionService
    {
        private readonly FromUrbanDbContext _context;
        private readonly IHistoryLogService _historyLogService;
        private readonly IAuthService _authService;
        private readonly UserManager<CostumIdentityUser> _userManager;
        private readonly IRolePermissionService _rolePermissionService;

        public UserPermissionService(FromUrbanDbContext context, IHistoryLogService historyLogService, IAuthService authService, UserManager<CostumIdentityUser> userManager, IRolePermissionService rolePermissionService)
        {
            _context = context;
            _historyLogService = historyLogService;
            _authService = authService;
            _userManager = userManager;
            _rolePermissionService = rolePermissionService;
        }

        public async Task<UserPermission> AddAsync(UserPermission entity)
        {
            await _context.UserPermission.AddAsync(entity);
            await _context.SaveChangesAsync();
            _historyLogService.PrepareForInsert($"ثبت دسترسی جدید برای کاربر {entity.User.Name} {entity.User.Family}",
                EnumFormName.UserPermission, EnumOperation.Post);
            return entity;
        }

        public async Task<List<UserPermission>> AddListAsync(List<UserPermission> entity)
        {
            try
            {
                await _context.UserPermission.AddRangeAsync(entity);
                await _context.SaveChangesAsync();
                //_historyLogService.PrepareForInsert($"ثبت دسترسی جدید برای کاربر {entity[0].User.Name} {entity[0].User.Family}",
                //   EnumFormName.UserPermission, EnumOperation.Post);
                return entity;
            }
            catch (Exception e)
            {
                _historyLogService.PrepareForInsert($"ثبت دسترسی جدید برای کاربر {entity[0].User.Name} {entity[0].User.Family}",
                    EnumFormName.UserPermission, EnumOperation.Post);
                throw;
            }
        }

        public Task<UserPermission> DeleteAsync(UserPermission entity)
        {
            throw new NotImplementedException();
        }

        public UserPermission DecryptInfo(UserPermission entity)
        {
            throw new NotImplementedException();
        }

        public UserPermission EncryptInfo(UserPermission entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<UserPermission> GetAsync(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyList<UserPermission>> GetAllAsync() =>
            await _context.UserPermission.Include(a => a.User).AsNoTracking().ToListAsync();


        public async Task<UserPermission> UpdateAsync(UserPermission entity)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteByUserIdAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return false;

            var userPermision = await GetAllByUserIdAsync(userId);
            _context.UserPermission.RemoveRange(userPermision);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<UserPermission>> GetAllByUserIdAsync(string userId) => await _context.UserPermission.Where(a => a.UserId == userId).ToListAsync();
        public async Task<List<UserPermission>> GetAllByUserIdAsync(CostumIdentityUser user)
        {
            var userPermission = await _context.UserPermission.Where(a => a.UserId == user.Id).ToListAsync();
            var userRoles = await _authService.GetRoleByUserIdAsync(user.Id);
            foreach (var userRole in userRoles)
            {
                var role = await _authService.GetRoleByNameAsync(userRole);
                var rolePermissions = await _rolePermissionService.GetAllByRoleIdAsync(role.Id);

                foreach (var rolePermission in rolePermissions)
                {
                    var anyPermission = userPermission.FirstOrDefault(x => x.PermissionId == rolePermission.PermissionId);
                    if (anyPermission == null)
                    {
                        var permission = new UserPermission
                        {
                            Identity = rolePermission.Identity,
                            UserId = user.Id,
                            PermissionId = rolePermission.PermissionId,
                            Hashed = rolePermission.Hashed
                        };
                        userPermission.Add(permission);
                    }

                }
            }

            return userPermission;
        }

        public async Task CheckChanges(CostumIdentityUser currentUser, CostumIdentityUser user, List<UserPermissionDto> permissions)
        {
            var dbPermissions = await GetAllByUserIdAsync(user.Id);

            // Create a HashSet for O(1) lookups instead of O(n) FirstOrDefault calls
            var dbPermissionIds = dbPermissions.Select(x => x.PermissionId).ToHashSet();

            // Find permissions that are being granted (Access = true) but don't exist in DB
            var added = permissions
                .Where(p => p.Access && !dbPermissionIds.Contains((int)p.Identity))
                .ToList();

            // Find permissions that are being revoked (Access = false) and exist in DB
            var deleted = permissions
                .Where(p => !p.Access && dbPermissionIds.Contains((int)p.Identity))
                .ToList();

            // Log changes in batch operations
            if (deleted.Count > 0)
            {
                var deleteMessages = deleted.Select(item =>
                    $"[دسترسی حذف شده = {item.Permission_Name}] - [کاربر انجام دهنده : {currentUser.UserName}] - [کاربر اعمال شده : {user.UserName}]");

                foreach (var message in deleteMessages)
                {
                    _historyLogService.PrepareForInsert(message, EnumFormName.UserPermission, EnumOperation.Delete);
                }
            }

            if (added.Count > 0)
            {
                var addMessages = added.Select(item =>
                    $"[دسترسی اضافه شده = {item.Permission_Name}] - [کاربر انجام دهنده : {currentUser.UserName}] - [کاربر اعمال شده : {user.UserName}]");

                foreach (var message in addMessages)
                {
                    _historyLogService.PrepareForInsert(message, EnumFormName.UserPermission, EnumOperation.Post);
                }
            }
        }

        public async Task DeleteByRoleName(CostumIdentityUser user, string[] roleNames)
        {
            try
            {
                var userPermissions = await GetAllByUserIdAsync(user.Id);
                if (!userPermissions.Any())
                    return;

                foreach (var roleName in roleNames)
                {
                    var role = await _authService.GetRoleByNameAsync(roleName);
                    var rolePermissions = await _rolePermissionService.GetAllByRoleIdAsync(role.Id);
                    foreach (var userPermission in rolePermissions.Select(roleP => userPermissions.Where(x => x.PermissionId == roleP.PermissionId).ToList()))
                    {
                        _context.UserPermission.RemoveRange(userPermission);
                    }
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return;
            }
        }

        public async Task<RolePermission> GetRoleAsync(long id)
        {
            return await _rolePermissionService.GetAsync(id);
        }
    }
}
