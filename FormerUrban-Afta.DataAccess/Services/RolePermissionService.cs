
using FormerUrban_Afta.DataAccess.DTOs.IdentityUser;
using System.Data;

namespace FormerUrban_Afta.DataAccess.Services;
public class RolePermissionService : IRolePermissionService
{
    private readonly FromUrbanDbContext _context;
    private readonly IHistoryLogService _historyLogService;

    public RolePermissionService(FromUrbanDbContext context, IHistoryLogService historyLogService)
    {
        _context = context;
        _historyLogService = historyLogService;
    }

    public Task<RolePermission> AddAsync(RolePermission entity)
    {
        throw new NotImplementedException();
    }

    public async Task<List<RolePermission>> AddListAsync(List<RolePermission> entity)
    {
        await _context.RolePermission.AddRangeAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public Task<RolePermission> DeleteAsync(RolePermission entity)
    {
        throw new NotImplementedException();
    }

    public RolePermission DecryptInfo(RolePermission entity)
    {
        throw new NotImplementedException();
    }

    public RolePermission EncryptInfo(RolePermission entity)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ExistAsync(long id)
    {
        throw new NotImplementedException();
    }

    public async Task<IReadOnlyList<RolePermission>> GetAllAsync() =>
    await _context.RolePermission.Include(a => a.Role).AsNoTracking().ToListAsync();

    public async Task<RolePermission> GetAsync(long id) => await _context.RolePermission.FirstOrDefaultAsync(x => x.Identity == id);

    public Task<RolePermission> UpdateAsync(RolePermission entity)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteByRoleIdAsync(string roleId)
    {
        if (string.IsNullOrWhiteSpace(roleId))
            return false;

        var rolePermision = await GetAllByRoleIdAsync(roleId);
        _context.RolePermission.RemoveRange(rolePermision);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<RolePermission>> GetAllByRoleIdAsync(string roleId) => await _context.RolePermission.Where(a => a.RoleId == roleId).ToListAsync();

    public async Task<List<RolePermissionDto>> CheckChanges(CostumIdentityUser currentUser, CostumIdentityRole role, List<RolePermissionDto> permissions)
    {
        var dbPermissions = await GetAllByRoleIdAsync(role.Id);

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
                $"[دسترسی حذف شده : {@item.PermissionName}] - [کاربر انجام دهنده : {@currentUser.UserName}] - [نقش اعمال شده : {@item.roleName}]");

            foreach (var message in deleteMessages)
            {
                _historyLogService.PrepareForInsert(message, EnumFormName.UserPermission, EnumOperation.Delete);
            }
        }

        if (added.Count > 0)
        {
            var addMessages = added.Select(item =>
                $"[دسترسی اضافه شده = {@item.PermissionName}] - [کاربر انجام دهنده : {@currentUser.UserName}] - [نقش اعمال شده : {@item.roleName}]");

            foreach (var message in addMessages)
            {
                _historyLogService.PrepareForInsert(message, EnumFormName.UserPermission, EnumOperation.Post);
            }
        }

        var result = new List<RolePermissionDto>();

        result.AddRange(added);
        result.AddRange(deleted);

        return result;
    }
}