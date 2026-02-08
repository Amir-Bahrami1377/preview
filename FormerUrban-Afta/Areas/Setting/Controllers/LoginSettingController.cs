using FormerUrban_Afta.Attributes;

namespace FormerUrban_Afta.Areas.Setting.Controllers
{
    [Area("Setting")]
    public class LoginSettingController : AmardBaseController
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ITarifhaService _tarifhaService;
        private readonly IHistoryLogService _historyLogService;
        private readonly IValidator<LoginSettingDto> _logSettingValidator;
        private readonly IAuthService _authService;


        public LoginSettingController(ITarifhaService tarifhaService, IHistoryLogService historyLogService,
            IValidator<LoginSettingDto> logSettingValidator,
            IServiceProvider serviceProvider, IAuthService authService)
        {
            _tarifhaService = tarifhaService;
            _historyLogService = historyLogService;
            _logSettingValidator = logSettingValidator;
            _serviceProvider = serviceProvider;
            _authService = authService;
        }

        [CheckUserAccess(permissionCode: "Menu_LoginSetting", type: EnumOperation.Get, table: EnumFormName.Tarifha, section: "مدیریت لاگین")]
        public async Task<IActionResult> Index()
        {
            var data = await _tarifhaService.GetLoginSetting();
            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckUserAccess(permissionCode: "LoginSetting_Edit", type: EnumOperation.Update, table: EnumFormName.Tarifha, section: "ثبت مدیریت لاگین")]
        public async Task<IActionResult> Submit(LoginSettingDto command)
        {
            var result = _logSettingValidator.Validate(command);
            if (!result.IsValid)
            {
                _historyLogService.PrepareForInsert($"خطای اعتبارسنجی در ویرایش مدیریت لاگین", EnumFormName.Tarifha, EnumOperation.Post);
                command.message = result.Errors.Select(e => e.ErrorMessage).ToList();
                return View("Index", command);
            }

            await _tarifhaService.UpdateLoginSetting(command);
            TempData["SuccessMessage"] = $"ویرایش مدیریت لاگین با موفقیت انجام شد.";
            return RedirectToAction("Index");
        }

        #region Account

        [CheckUserAccess(permissionCode: "Menu_Account", type: EnumOperation.Get, table: EnumFormName.UserSession, section: "مدیریت نسشت های کاربر")]
        public async Task<IActionResult> Account()
        {
            var currentUser = _authService.GetCurrentUser();
            var roles = await _authService.GetRoleByUserIdAsync(currentUser.Id);
            var isAdmin = roles.Any(r => r.Equals("Administrator", StringComparison.OrdinalIgnoreCase));

            var data = new List<UserSessionDto>();
            var usersessionService = _serviceProvider.GetService<IUserSessionService>();

            if (isAdmin)
                data = await usersessionService.GetAllAsync();
            else
                data = await usersessionService.GetByUserAsync(currentUser.Id);
            foreach (var item in data)
            {
                if (!item.IsValid)
                {
                    _historyLogService.PrepareForInsert($"رد صحت سنجی داده سشن های فعال کاربر{currentUser.UserName}",
                        EnumFormName.UserSession, EnumOperation.Validate);
                }
            }
            _historyLogService.PrepareForInsert($"مشاهده اطلاعات مدیریت نشست های کاربران", EnumFormName.UserSession, EnumOperation.Get);
            return View(data);
        }

        [CheckUserAccess(permissionCode: "Reports_DeleteUserSession", type: EnumOperation.Get, table: EnumFormName.UserSession, section: "حذف نشست های کاربران")]
        public async Task<IActionResult> DeleteAccount(Guid id)
        {
            var userSessionService = _serviceProvider.GetService<IUserSessionService>();
            var res = await userSessionService.Delete(id);
            if (res.Success)
            {
                _historyLogService.PrepareForInsert($"عملیات حذف نشت کاربر {res.UserName} با موفقیت انجام شد!", EnumFormName.UserSession, EnumOperation.Delete);
                TempData["SuccessMessage"] = "عملیات حذف نشست با موفقیت انجام شد!";
            }
            else
            {
                _historyLogService.PrepareForInsert($"عملیات حذف نشست کاربر {res.UserName} با خطا مواجه شد!", EnumFormName.UserSession, EnumOperation.Delete);
                ViewBag.ErrorMessage = $"{res.Message}";
            }
            return RedirectToAction("Account");
        }

        #endregion
    }
}
