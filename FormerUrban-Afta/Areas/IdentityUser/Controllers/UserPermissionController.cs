using FormerUrban_Afta.Attributes;

namespace FormerUrban_Afta.Areas.IdentityUser.Controllers
{
    [Area("IdentityUser")]
    public class UserPermissionController : AmardBaseController
    {
        private readonly ILogger<UserPermissionController> _logger;
        private readonly IUserPermissionService _userPermissionService;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        private readonly IHistoryLogService _historyLogService;

        public UserPermissionController(ILogger<UserPermissionController> logger, IUserPermissionService userPermissionService, IMapper mapper, IAuthService authService, IHistoryLogService historyLogService)
        {
            _logger = logger;
            _userPermissionService = userPermissionService;
            _mapper = mapper;
            _authService = authService;
            _historyLogService = historyLogService;
        }

        [CheckUserAccess(permissionCode: "Menu_UserPermission", type: EnumOperation.Get, table: EnumFormName.UserPermission, section: "دسترسی کاربران")]
        public async Task<IActionResult> Index()
        {
            var users = await _authService.GetAllAsync();
            _historyLogService.PrepareForInsert("مشاهده اطلاعات دسترسی کاربران", EnumFormName.UserPermission, EnumOperation.Get);

            var result = users.Select(x => new UserDto()
            {
                Id = x.Id,
                Name = x.Name,
                Family = x.Family,
                UserName = x.UserName ?? "",
                PhoneNumber = x.PhoneNumber ?? "",
                Email = x.Email ?? "",
                LockoutEnabled = x.LockoutEnabled,
                TwoFactorEnabled = x.TwoFactorEnabled,
                IsValid = CipherService.IsEqual(x.ToString(), x.Hashed),
            }).ToList();

            var valid = result.Where(x => !x.IsValid).ToList();
            if (valid.Any())
            {
                foreach (var item in valid)
                {
                    _historyLogService.PrepareForInsert($"رد صحت سنجی داده کاربر {item.Name} {item.Family}", EnumFormName.AspNetUsers, EnumOperation.Validate);
                }
            }

            return View(model: result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckUserAccess(permissionCode: "Menu_UserPermission", type: EnumOperation.Get, table: EnumFormName.UserPermission, section: "دسترسی کاربران")]
        public async Task<PartialViewResult> GetAllPermission(string userName)
        {
            var user = await _authService.GetByUserNameAsync(userName);
            var userPermissions = await _userPermissionService.GetAllByUserIdAsync(user);

            var allEnumPermissions = Enum.GetValues(typeof(EnumPermission)).Cast<EnumPermission>()
                .Select(e => new UserPermissionDto
                {
                    PermissionId = (int)e,
                    Permission_Description = e.GetType().GetMember(e.ToString()).First().GetCustomAttribute<DisplayAttribute>()?.Name ?? e.ToString(),
                    Access = userPermissions.Any(up => up.PermissionId == (int)e),
                    UserId = user.Id,
                    CostumIdentityUser_Name = user.Name,
                    CostumIdentityUser_Family = user.Family,
                }).ToList();

            foreach (var permission in userPermissions)
            {
                var p = allEnumPermissions.FirstOrDefault(x => x.PermissionId == permission.PermissionId);
                if (p != null)
                {
                    var valid = CipherService.IsEqual(permission.ToString(), permission.Hashed);
                    if (!valid)
                    {
                        var roleP = await _userPermissionService.GetRoleAsync(permission.Identity);
                        if (roleP != null)
                            valid = CipherService.IsEqual(roleP.ToString(), roleP.Hashed);
                    }

                    p.IsValid = valid;
                    if (!p.IsValid)
                        _historyLogService.PrepareForInsert($"رد صحت سنجی داده دسترسی کاربران با عنوان {MyFunction2.GetDisplayValue<EnumPermission>(permission.PermissionId)}", EnumFormName.AspNetUsers, EnumOperation.Validate);
                }
            }

            _historyLogService.PrepareForInsert($"مشاهده دسترسی کاربر {user.Family} {user.Name}", EnumFormName.UserPermission, EnumOperation.Get);

            return PartialView(allEnumPermissions);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckUserAccess(permissionCode: "UserPermission_Edit", type: EnumOperation.Update, table: EnumFormName.UserPermission, section: "ویرایش اطلاعات دسترسی کاربران")]
        public async Task<IActionResult> UpdateUserPermissions([FromBody] List<UserPermissionDto> permissions)
        {
            if (permissions == null || !permissions.Any())
            {
                _historyLogService.PrepareForInsert(description: "خطا در بخش ذخیره اطلاعات دسترسی کاربران.", formName: EnumFormName.AspNetUsers, operation: EnumOperation.Update);
                return new JsonResult(new { success = false, message = "" });
            }

            var userName = permissions[0].UserName;
            var removeUserPermision = permissions;
            var currentUser = _authService.GetCurrentUser();
            var user = await _authService.GetByUserNameAsync(userName);

            await _userPermissionService.CheckChanges(currentUser, user, permissions);

            foreach (var item in permissions.Where(a => a.Access == false).ToList())
            {
                removeUserPermision.Remove(item);
            }

            permissions = removeUserPermision;

            var userPermision = _mapper.Map<List<UserPermission>>(permissions);

            userPermision.ForEach(a => a.UserId = user.Id);
            userPermision.ForEach(a => a.PermissionId = (int)a.Identity);
            userPermision.ForEach(a => a.Identity = 0);

            await _userPermissionService.DeleteByUserIdAsync(user.Id);
            await _userPermissionService.AddListAsync(userPermision);
            TempData["SuccessMessage"] = "عملیات با موفقیت انجام شد.";
            return new JsonResult(new { success = true, message = "" });
        }
    }
}
