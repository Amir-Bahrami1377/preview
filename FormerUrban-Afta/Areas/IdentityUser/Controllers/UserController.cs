using FormerUrban_Afta.Attributes;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace FormerUrban_Afta.Areas.IdentityUser.Controllers
{
    [Area("IdentityUser")]
    public class UserController : AmardBaseController
    {
        private readonly ILogger<UserController> _logger;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;
        private readonly UserManager<CostumIdentityUser> _userManager;
        private readonly SignInManager<CostumIdentityUser> _signInManager;
        private readonly ExtensionUser _extensionUser;
        private readonly IHistoryLogService _historyLogService;
        private readonly IValidator<CreateCostumIdentityUserDto> _createCostumIdentityUserValidator;
        private readonly IValidator<UpdateCostumIdentityUserDto> _updateCostumIdentityUserValidator;
        private readonly IValidator<ChangePasswordDto> _changePasswordValidator;
        private readonly ISendSmsService _sendSmsService;
        private readonly IUserPermissionService _userPermissionService;
        private readonly IAuditService _auditService;

        public UserController(ILogger<UserController> logger, IAuthService authService, IMapper mapper, UserManager<CostumIdentityUser> userManager,
            SignInManager<CostumIdentityUser> signInManager, ExtensionUser extensionUser, IHistoryLogService historyLogService,
            IValidator<CreateCostumIdentityUserDto> createCostumIdentityUserValidator, IValidator<UpdateCostumIdentityUserDto> updateCostumIdentityUserValidator,
            IValidator<ChangePasswordDto> changePasswordValidator, ISendSmsService sendSmsService, IUserPermissionService userPermissionService, IAuditService auditService)
        {
            _logger = logger;
            _authService = authService;
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _extensionUser = extensionUser;
            _historyLogService = historyLogService;
            _createCostumIdentityUserValidator = createCostumIdentityUserValidator;
            _updateCostumIdentityUserValidator = updateCostumIdentityUserValidator;
            _changePasswordValidator = changePasswordValidator;
            _sendSmsService = sendSmsService;
            _userPermissionService = userPermissionService;
            _auditService = auditService;
        }

        [CheckUserAccess(permissionCode: "Menu_User", type: EnumOperation.Get, table: EnumFormName.AspNetUsers, section: "دسترسی کاربران")]
        public async Task<IActionResult> Index()
        {
            var users = await _authService.GetAllAsync();
            _historyLogService.PrepareForInsert($"مشاهده اطلاعات کاربران", EnumFormName.AspNetUsers, EnumOperation.Get);
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
                    _historyLogService.PrepareForInsert($"رد صحت سنجی داده کاربر {item.Name} {item.Family}", EnumFormName.AspNetUsers, EnumOperation.Get);
                }
            }

            return View(model: result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckUserAccess(permissionCode: "UserManage_CreateUser", type: EnumOperation.Get, table: EnumFormName.AspNetUsers, section: "ایجاد کاربران")]
        public async Task<IActionResult> CreateUser()
        {
            var result = new CreateCostumIdentityUserDto
            {
                Roles = (await _authService.GetAllRoleAsync()).ToList()
            };
            _historyLogService.PrepareForInsert($"مشاهده اطلاعات ایجاد کاربر جدید", EnumFormName.AspNetUsers, EnumOperation.Get);
            return PartialView(model: result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckUserAccess(permissionCode: "UserManage_CreateUser", type: EnumOperation.Post, table: EnumFormName.AspNetUsers, section: "ایجاد کاربران")]
        public async Task<IActionResult> CreateUserSubmit(CreateCostumIdentityUserDto identityUser)
        {
            ValidationResult validate = _createCostumIdentityUserValidator.Validate(identityUser);
            if (!validate.IsValid)
            {
                _historyLogService.PrepareForInsert(description: $"خطا در اعتبار سنجی ایجار کاربر جدید.", formName: EnumFormName.AspNetUsers, operation: EnumOperation.Post);
                var errorMessages = validate.Errors.Select(e => e.ErrorMessage).ToList();
                return new JsonResult(new { success = false, message = errorMessages });
            }

            // password check
            var resultPassword = await _extensionUser.CheckPasswordValidation(identityUser.PasswordHash);
            if (!resultPassword.IsOk)
                return new JsonResult(new { success = false, message = resultPassword.Message });

            var validation = ValidationPassword.IsValidPassword(identityUser.PasswordHash, identityUser.PhoneNumber);
            if (!validation.Success)
            {
                _historyLogService.PrepareForInsert(description: $"خطا در اعتبار سنجی ایجاد کاربر جدید.", formName: EnumFormName.AspNetUsers, operation: EnumOperation.Post);
                return new JsonResult(new { success = false, message = validation.Message });
            }

            // user Mobile Check
            bool existsPhoneNumber = await _userManager.Users.AnyAsync(u => u.PhoneNumber == identityUser.PhoneNumber);

            if (existsPhoneNumber)
            {
                _historyLogService.PrepareForInsert(description: $"وارد کردن شماره همراه تکراری در ایجاد کاربر {identityUser.Name} {identityUser.Family}.", formName: EnumFormName.AspNetUsers, EnumOperation.Post);
                return new JsonResult(new { success = false, message = "شماره موبایل قبلا استفاده شده است." });
            }

            CostumIdentityUser user = _mapper.Map<CostumIdentityUser>(identityUser);
            user.EmailConfirmed = true;
            IdentityResult result = await _authService.CreateAsync(user);

            if (!result.Succeeded)
            {
                foreach (var item in result.Errors.Where(a => a.Code.Contains("DuplicateUserName")))
                {
                    item.Description = $"نام کاربری {user.UserName} موجود می باشد";
                }

                _historyLogService.PrepareForInsert($"خطا در ثبت کاربر {user.Name} {user.Family}", EnumFormName.AspNetUsers, EnumOperation.Post);
                var errors = result.Errors.Select(e => e.Description).ToArray();
                return new JsonResult(new { success = false, message = errors });
            }

            foreach (var role in identityUser.Role)
            {
                await _authService.AddToRoleAsync(user, role);
            }

            var sms = await _sendSmsService.SendMessageSmsWithRespondToSuperusers2("با موفقیت ایجاد", "کاربر گرامی حساب شما با موفقیت ایجاد شده است.", user.Id, 351500);
            if (sms.StrRetStatus != "با موفقیت ارسال شد")
                TempData["ValidateMessage"] = "خطا در ارسال پیامک.";

            _historyLogService.PrepareForInsert($"کاربر {identityUser.UserName} با موفقیت اضافه شد.", EnumFormName.AspNetUsers, EnumOperation.Post);
            TempData["SuccessMessage"] = $"کاربر با نام کاربری {identityUser.UserName} با موفقیت ایجاد شد.";
            return new JsonResult(new { success = true });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckUserAccess(permissionCode: "UserManage_EditUser", type: EnumOperation.Get, table: EnumFormName.AspNetUsers, section: "ویرایش کاربران")]
        public async Task<IActionResult> UpdateUser(string userName)
        {
            var user = await _authService.GetByUserNameAsync(userName);
            var model = _mapper.Map<UpdateCostumIdentityUserDto>(user);
            model.PasswordHash = "";
            model.Role = await _authService.GetRoleByUserIdAsync(model.Id ?? "");
            model.Roles = (await _authService.GetAllRoleAsync()).ToList();
            _historyLogService.PrepareForInsert($"مشاهده ویرایش اطلاعات کاربر : {model.Name} {model.Family}", EnumFormName.AspNetUsers, EnumOperation.Get);
            return PartialView(model: model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckUserAccess(permissionCode: "UserManage_EditUser", type: EnumOperation.Update, table: EnumFormName.AspNetUsers, section: "ویرایش کاربران")]
        public async Task<IActionResult> UpdateUserSubmit(UpdateCostumIdentityUserDto identityUser)
        {
            var validate = _updateCostumIdentityUserValidator.Validate(identityUser);
            if (!validate.IsValid)
            {
                _historyLogService.PrepareForInsert(description: $"خطا در اعتبار سنجی ویرایش کاربر {identityUser.Name} {identityUser.Family}.", formName: EnumFormName.AspNetUsers, EnumOperation.Update);
                var errorMessages = validate.Errors.Select(e => e.ErrorMessage).ToList();
                return new JsonResult(new { success = false, message = errorMessages });
            }

            if (!string.IsNullOrWhiteSpace(identityUser.PasswordHash))
            {
                var resultPassword = await _extensionUser.CheckPasswordValidation(identityUser.PasswordHash);
                if (!resultPassword.IsOk)
                    return new JsonResult(new { success = false, message = resultPassword.Message });

                var validation = ValidationPassword.IsValidPassword(identityUser.PasswordHash, identityUser.PhoneNumber);
                if (!validation.Success)
                {
                    _historyLogService.PrepareForInsert(description: $"خطا در اعتبار سنجی ویرایش کاربر {identityUser.Name} {identityUser.Family}.", formName: EnumFormName.AspNetUsers, EnumOperation.Update);
                    return new JsonResult(new { success = false, message = validation.Message });
                }
            }

            // user Mobile Check
            bool existsPhoneNumber = await _userManager.Users.AnyAsync(u => u.PhoneNumber == identityUser.PhoneNumber && u.Id != identityUser.Id);

            if (existsPhoneNumber)
            {
                _historyLogService.PrepareForInsert(description: $"وارد کردن شماره همراه تکراری در ایجاد کاربر {identityUser.Name} {identityUser.Family}.", formName: EnumFormName.AspNetUsers, EnumOperation.Post);
                return new JsonResult(new { success = false, message = "شماره موبایل قبلا استفاده شده است." });
            }

            var user = _mapper.Map<CostumIdentityUser>(identityUser);
            var oldModel = await _authService.GetByUserNameAsNoTrackingAsync(identityUser.OldUserName ?? "");
            var userModel = await _authService.GetByUserNameAsync(identityUser.OldUserName ?? "");

            if (userModel.UserName == null)
            {
                _historyLogService.PrepareForInsert(description: $"کاربر {identityUser.Name} {identityUser.Family} نامعتبر میباشد.", formName: EnumFormName.AspNetUsers, EnumOperation.Update);
                return new JsonResult(new { success = false, message = "کاربر نامعتبر می باشد" });
            }

            userModel.Name = identityUser.Name;
            userModel.Family = identityUser.Family;
            userModel.UserName = identityUser.UserName;
            userModel.Email = identityUser.Email;
            userModel.PhoneNumber = identityUser.PhoneNumber;

            var result = await _authService.UpdateAsync(userModel);
            user = await _authService.GetByUserNameAsync(identityUser.UserName);
            if (result.Succeeded && !string.IsNullOrEmpty(identityUser.PasswordHash))
            {
                // تولید توکن ریست رمز عبور
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                // تغییر رمز عبور با توکن
                result = await _userManager.ResetPasswordAsync(user, token, identityUser.PasswordHash);
            }

            if (!result.Succeeded)
            {
                _historyLogService.PrepareForInsert(description: $"خطا در ویرایش اطلاعات کاربر {identityUser.Name} {identityUser.Family}", formName: EnumFormName.AspNetUsers, EnumOperation.Update);
                var errors = result.Errors.Select(e => e.Description).ToArray();
                return new JsonResult(new { success = false, message = errors });
            }
            //model.Role = await _authService.GetRoleByUserIdAsync(model.Id);
            List<CostumIdentityRole> rolesList = (await _authService.GetAllRoleAsync()).ToList();
            var roleNames = await _authService.GetRoleByUserIdAsync(userModel.Id);
            var deleteRoles = roleNames.Except(identityUser.Role).ToArray();
            var deleteRoleNames = string.Join(" , ",
                deleteRoles.Select(x => rolesList.FirstOrDefault(y => y.Name == x)!.Description));

            await _userManager.RemoveFromRolesAsync(user, roleNames);
            if (deleteRoles.Length > 0)
            {
                await _userPermissionService.DeleteByRoleName(user, deleteRoles);
                _historyLogService.PrepareForInsert(description: $"نقش های حذف شده کاربر {identityUser.Name} {identityUser.Family} : {deleteRoleNames}", formName: EnumFormName.AspNetUsers, EnumOperation.Update);
            }

            foreach (var role in identityUser.Role)
            {
                await _authService.AddToRoleAsync(user, role);
            }

            if (identityUser.Role.Count > 0)
            {
                var addRoleNames = string.Join(" , ",
                    identityUser.Role.Select(x => rolesList.FirstOrDefault(y => y.Name == x)!.Description));
                _historyLogService.PrepareForInsert(description: $"نقش های اضافه شده کاربر {identityUser.Name} {identityUser.Family} : {addRoleNames}", formName: EnumFormName.AspNetUsers, EnumOperation.Update);
            }

            await _userManager.UpdateSecurityStampAsync(user);
            _auditService.GetDifferences<CostumIdentityUser>(oldModel, user, oldModel.Id, EnumFormName.AspNetUsers, EnumOperation.Update);
            var sms = await _sendSmsService.SendMessageSmsWithRespondToSuperusers2("با موفقیت ویرایش", "کاربر گرامی حساب شما با موفقیت ویرایش شده است.", user.Id, 351500);
            if (sms.StrRetStatus != "با موفقیت ارسال شد")
                TempData["ValidateMessage"] = "خطا در ارسال پیامک.";
            _historyLogService.PrepareForInsert(description: $"ویرایش اطلاعات کاربر {identityUser.Name} {identityUser.Family} با موفقیت انجام شد.", formName: EnumFormName.AspNetUsers, EnumOperation.Update);
            TempData["SuccessMessage"] = "کاربر با موفقیت ویرایش شد";
            return new JsonResult(new { success = true });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckUserAccess(permissionCode: "UserChangePassword", type: EnumOperation.Get, table: EnumFormName.AspNetUsers, section: "تغییر رمز عبور کاربر")]
        public IActionResult ChangePassword()
        {
            _historyLogService.PrepareForInsert("نمایش اطلاعات تغییر رمز عبور کاربر جاری", EnumFormName.AspNetUsers, EnumOperation.Get);
            return PartialView(new ChangePasswordDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckUserAccess(permissionCode: "UserChangePassword", type: EnumOperation.Update, table: EnumFormName.AspNetUsers, section: "تغییر رمز عبور کاربر")]
        public async Task<IActionResult> ChangePasswordSubmit(ChangePasswordDto model)
        {
            ValidationResult validate = _changePasswordValidator.Validate(model);
            if (!validate.IsValid)
            {
                _historyLogService.PrepareForInsert(description: "خطا در اعتبار سنجی تغییر رمز عبور کاربر", formName: EnumFormName.AspNetUsers, EnumOperation.Update);
                var errorMessages = validate.Errors.Select(e => e.ErrorMessage).ToList();
                return new JsonResult(new { success = false, message = errorMessages });
            }

            var user = await _authService.GetCurentUserAsync();
            //var check = await _signInManager.PasswordSignInAsync(user.UserName ?? "", model.OldPassword, false, lockoutOnFailure: false);
            //if (!check.Succeeded)
            //{
            //    _historyLogService.PrepareForInsert(description: $"وارد کردن رمز عبور قدیم نادرست در قسمت تغییر رمز عبور کاربر {user.Name} {user.Family}", formName: EnumFormName.AspNetUsers, EnumOperation.Update);
            //    return new JsonResult(new { success = false, message = "رمز عبور قدیم اشتباه است!!" });
            //}

            // 1. Check if the old password is correct
            var isOldPasswordCorrect = await _userManager.CheckPasswordAsync(user, model.OldPassword);
            if (!isOldPasswordCorrect)
            {
                _historyLogService.PrepareForInsert(description: $"وارد کردن رمز عبور قدیم نادرست در قسمت تغییر رمز عبور کاربر {user.Name} {user.Family}", formName: EnumFormName.AspNetUsers, EnumOperation.Update);
                return new JsonResult(new { success = false, message = "رمز عبور قدیم اشتباه است!!" });
            }

            var resultPassword = await _extensionUser.CheckPasswordValidation(model.NewPassword);
            if (!resultPassword.IsOk)
                return new JsonResult(new { success = false, message = resultPassword.Message });

            var validation = ValidationPassword.IsValidPassword(model.NewPassword, user.PhoneNumber ?? "", model.OldPassword);
            if (!validation.Success)
            {
                _historyLogService.PrepareForInsert(description: validation.Message, formName: EnumFormName.AspNetUsers, EnumOperation.Update);
                return new JsonResult(new { success = false, message = validation.Message });
            }

            //await _userManager.RemovePasswordAsync(user);
            //var result = await _userManager.AddPasswordAsync(user, model.NewPassword);
            // تولید توکن ریست رمز عبور
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            // تغییر رمز عبور با توکن
            var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);
            if (result.Succeeded)
            {
                await _userManager.UpdateSecurityStampAsync(user);
                _historyLogService.PrepareForInsert(description: $"رمز عبور کاربر {model.UserName} با موفقیت تغییر کرد.", formName: EnumFormName.AspNetUsers, EnumOperation.Update);
                var sUser = await _userManager.FindByNameAsync(user.UserName);
                if (sUser != null)
                {
                    var sms = await _sendSmsService.SendMessageSmsWithRespondToSuperusers2(user.Family, "کاربر گرامی رمز عبور شما با موفقیت تغییر کرد.", sUser.Id, 351506);
                    if (sms.StrRetStatus != "با موفقیت ارسال شد")
                        TempData["ValidateMessage"] = "خطا در ارسال پیامک.";
                }
                TempData["SuccessMessage"] = "کاربر گرامی رمز عبور شما با موفقیت تغییر کرد.";
                return new JsonResult(new { success = true });
            }

            _historyLogService.PrepareForInsert(description: "خطا در تغییر رمز عبور کاربر", formName: EnumFormName.AspNetUsers, EnumOperation.Update);
            var message = result.Errors.Select(e => e.Description).ToArray();
            return new JsonResult(new { success = false, message = message });
        }

        [CheckUserAccess(permissionCode: "UserManage_BlockUser", type: EnumOperation.Update, table: EnumFormName.AspNetUsers, section: "مسدود کردن کاربران")]
        public async Task<IActionResult> BlockUser(string userName)
        {
            var result = await _authService.BlockUser(userName);
            if (!result)
                _historyLogService.PrepareForInsert(description: $"عملیات مسدود کردن کاربر {userName} با خطا مواجه شد.", formName: EnumFormName.AspNetUsers, operation: EnumOperation.Update);
            else
            {
                _historyLogService.PrepareForInsert(description: $"کاربر {userName} با موفقیت مسدود شد.", formName: EnumFormName.AspNetUsers, operation: EnumOperation.Update);
                TempData["SuccessMessage"] = $"کاربر {userName} با موفقیت غیرفعال شد!";
                var user = await _userManager.FindByNameAsync(userName);
                if (user != null)
                {
                    var sms = await _sendSmsService.SendMessageSmsWithRespondToSuperusers2("مسدود", "کاربر گرامی حساب شما مسدود شده است.", user.Id, 351500);
                    if (sms.StrRetStatus != "با موفقیت ارسال شد")
                        TempData["ValidateMessage"] = "خطا در ارسال پیامک.";
                }
            }
            return RedirectToAction("Index");
        }

        [CheckUserAccess(permissionCode: "UserManage_BlockUser", type: EnumOperation.Update, table: EnumFormName.AspNetUsers, section: "فعالسازی کاربران")]
        public async Task<IActionResult> UnBlockUser(string userName)
        {
            var result = await _authService.UnBlockUser(userName);
            if (!result.Success)
            {
                _historyLogService.PrepareForInsert(description: $"عملیات فعالسازی کاربر {userName} با خطا مواجه شد.", formName: EnumFormName.AspNetUsers, operation: EnumOperation.Update);
                TempData["ValidateMessage"] = result.Message;
            }
            else
            {
                _historyLogService.PrepareForInsert(description: $"کاربر {userName} با موفقیت فعال شد.", formName: EnumFormName.AspNetUsers, operation: EnumOperation.Update);
                TempData["SuccessMessage"] = $"کاربر {userName} با موفقیت فعال شد.";
                var user = await _userManager.FindByNameAsync(userName);
                if (user != null)
                {
                    var sms = await _sendSmsService.SendMessageSmsWithRespondToSuperusers2("فعال", "کاربر گرامی حساب شما فعال شده است.", user.Id, 351500);
                    if (sms.StrRetStatus != "با موفقیت ارسال شد")
                        TempData["ValidateMessage"] = "خطا در ارسال پیامک.";
                }
            }

            return RedirectToAction("Index");
        }
    }
}
