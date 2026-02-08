using FormerUrban_Afta.Attributes;

namespace FormerUrban_Afta.Areas.IdentityUser.Controllers
{
    [Area("IdentityUser")]

    public class RolePermissionController : AmardBaseController
    {
        private readonly ILogger<RolePermissionController> _logger;
        private readonly IRolePermissionService _rolePermissionService;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        private readonly IHistoryLogService _historyLogService;

        public RolePermissionController(ILogger<RolePermissionController> logger, IRolePermissionService rolePermissionService, IMapper mapper
            , IAuthService authService, IHistoryLogService historyLogService)
        {
            _logger = logger;
            _rolePermissionService = rolePermissionService;
            _mapper = mapper;
            _authService = authService;
            _historyLogService = historyLogService;
        }

        [CheckUserAccess(permissionCode: "Menu_RolePermission", type: EnumOperation.Get, table: EnumFormName.RolePermission, section: "دسترسی نقش ها")]
        public async Task<IActionResult> Index()
        {
            var roles = await _authService.GetAllRoleAsync();
            _historyLogService.PrepareForInsert("مشاهده اطلاعات درسترسی نقش ها", EnumFormName.RolePermission, EnumOperation.Get);
            var result = roles.Select(x => new RoleDto()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                IsValid = CipherService.IsEqual(x.ToString(), x.Hashed),
            }).ToList();

            var valid = result.Where(x => !x.IsValid).ToList();
            if (valid.Any())
            {
                foreach (var item in valid)
                {
                    _historyLogService.PrepareForInsert($"رد صحت سنجی داده نقش با عنوان {item.Description}", EnumFormName.AspNetUsers, EnumOperation.Post);
                }
            }
            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckUserAccess(permissionCode: "Menu_RolePermission", type: EnumOperation.Post, table: EnumFormName.RolePermission, section: "دسترسی نقش ها")]
        public async Task<PartialViewResult> GetAllPermission(string roleName)
        {
            var role = await _authService.GetRoleByNameAsync(roleName);
            var rolePermissions = _rolePermissionService.GetAllByRoleIdAsync(role.Id).Result;

            var allEnumPermissions = Enum.GetValues(typeof(EnumPermission))
                .Cast<EnumPermission>()
                .Select(e => new RolePermissionDto
                {
                    PermissionId = (int)e,
                    PermissionName = e.GetType().GetMember(e.ToString()).First().GetCustomAttribute<DisplayAttribute>()?.Name ?? e.ToString(),
                    Access = rolePermissions.Any(up => up.PermissionId == (int)e),
                })
                .ToList();

            foreach (var role1 in rolePermissions)
            {
                var p = allEnumPermissions.FirstOrDefault(x => x.PermissionId == role1.PermissionId);
                if (p != null)
                {
                    p.IsValid = CipherService.IsEqual(role1.ToString(), role1.Hashed);
                    if (!p.IsValid)
                        _historyLogService.PrepareForInsert($"رد صحت سنجی داده دسترسی نقش ها با عنوان {MyFunction2.GetDisplayValue<EnumPermission>(role1.PermissionId)}", EnumFormName.AspNetUsers, EnumOperation.Validate);
                }
            }

            _historyLogService.PrepareForInsert($"مشاهده اطلاعات درسترسی نقش {role.Description}", EnumFormName.RolePermission, EnumOperation.Get);
            return PartialView(allEnumPermissions);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckUserAccess(permissionCode: "RolePermission_Eidt", type: EnumOperation.Update, table: EnumFormName.RolePermission, section: "ویرایش اطلاعات دسترسی نقش ها")]
        public async Task<IActionResult> UpdateUserPermissions([FromBody] List<RolePermissionDto> permissions)
        {
            if (permissions == null || !permissions.Any())
                return new JsonResult(new { success = false, message = "" });

            var roleName = permissions[0].roleName;
            var role = await _authService.GetRoleByNameAsync(roleName);
            var removeUserPermision = permissions;
            var currentUser = _authService.GetCurrentUser();
            await _rolePermissionService.CheckChanges(currentUser, role, permissions);
            foreach (var item in permissions.Where(a => a.Access == false).ToList())
            {
                removeUserPermision.Remove(item);
            }

            permissions = removeUserPermision;

            var rolePermision = _mapper.Map<List<RolePermission>>(permissions);
            rolePermision.ForEach(a => a.RoleId = role.Id);
            rolePermision.ForEach(a => a.PermissionId = (int)a.Identity);
            rolePermision.ForEach(a => a.Identity = 0);

            await _rolePermissionService.DeleteByRoleIdAsync(role.Id);
            await _rolePermissionService.AddListAsync(rolePermision);

            TempData["SuccessMessage"] = "عملیات با موفقیت انجام شد.";
            return new JsonResult(new { success = true, message = "" });
        }
    }
}
