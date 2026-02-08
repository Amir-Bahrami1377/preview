using FormerUrban_Afta.DataAccess.DTOs.Login;
using FormerUrban_Afta.DataAccess.Services.Sms;
using Microsoft.AspNetCore.Authorization;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace FormerUrban_Afta.Controllers
{
    public class LoginController : AmardBaseController
    {
        private readonly IAuthService _authenticateService;
        private readonly UserManager<CostumIdentityUser> _userManager;
        private readonly MelipayamakSmsService _melipayamakSmsService;
        private readonly IEventLogFilterService _eventLogFilterService;
        private readonly IHistoryLogService _historyLogService;
        private readonly IBrowserService _browserService;
        private readonly IIpService _ipService;
        private readonly IUserLoginedService _userLoginedService;
        private readonly IEventLogThresholdService _eventLogThresholdService;
        private readonly IValidator<AuthRequest> _loginValidator;
        private readonly IValidator<ChangePasswordDto> _changePasswordValidator;
        private readonly ISendSmsService _sendSmsService;
        public readonly ExtensionUser _ExtensionUser;
        public readonly ITarifhaService _TarifhaService;

        public LoginController(IAuthService authenticateService, UserManager<CostumIdentityUser> userManager, MelipayamakSmsService melipayamakSmsService,
            IEventLogFilterService eventLogFilterService, IHistoryLogService historyLogService, IBrowserService browserService, IIpService ipService,
            IUserLoginedService userLoginedService, IEventLogThresholdService eventLogThresholdService, IValidator<AuthRequest> loginValidator,
            IValidator<ChangePasswordDto> changePasswordValidator, ISendSmsService sendSmsService, ExtensionUser extensionUser, ITarifhaService tarifhaService)
        {
            _authenticateService = authenticateService;
            _userManager = userManager;
            _melipayamakSmsService = melipayamakSmsService;
            _eventLogFilterService = eventLogFilterService;
            _historyLogService = historyLogService;
            _browserService = browserService;
            _ipService = ipService;
            _userLoginedService = userLoginedService;
            _eventLogThresholdService = eventLogThresholdService;
            _loginValidator = loginValidator;
            _changePasswordValidator = changePasswordValidator;
            _sendSmsService = sendSmsService;
            _ExtensionUser = extensionUser;
            _TarifhaService = tarifhaService;
        }

        #region Login

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            await _authenticateService.SeedRolesAsync();
            await _authenticateService.CreateDeveloperUser();

            if (TempData.ContainsKey("ChangePasswordSuccess") && (bool)(TempData["ChangePasswordSuccess"] ?? false))
                ViewBag.message = "-";
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> TwoStepLogin(AuthRequest login)
        {
            var result = await _loginValidator.ValidateAsync(login, options => options.IncludeRuleSets("Login"));
            if (!result.IsValid)
            {
                _historyLogService.PrepareForInsert($"خطا اعتبار سنجی در ورود کاربر {login.UserName}", EnumFormName.UserLogined, EnumOperation.Post);
                ViewBag.message = string.Join(" / ", result.Errors.Select(e => e.ErrorMessage).ToArray());
                InsertUserLoginLog(Strings.Persian.Fields.BadLogin, login.UserName, "", "", UserLoginStatus.Failed);
                return View("Index", login);
            }

            var check = await _authenticateService.CheckLogin(login);
            var user = await _userManager.FindByNameAsync(login.UserName);

            switch (check.Success)
            {
                case false when check.Message == "TwoFactorEnabled":
                    return View("OTP");

                case false when check.Message == "SmsEnabled" && user != null:

                    var validSms = await SendSmsMessage(user);
                    if (validSms)
                    {
                        login.ExpireTime = TempData["SMSExpirationDateTimeGregorian"]?.ToString();
                        login.UserName = user?.UserName ?? "";
                        login.CaptchaCode = "";
                        return View("Sms", login);
                    }

                    TempData.Clear();
                    ViewBag.message = "خطا در ارسال پیامک.";
                    return View("Index");

                default:
                    InsertUserLoginLog(check.Message, user?.UserName ?? login.UserName, user?.Id ?? "", $"{user?.Name ?? ""} {user?.Family ?? ""}", UserLoginStatus.Failed);
                    ViewBag.message = check.Message;
                    return View("Index");
            }
        }

        [ValidateAntiForgeryToken]
        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> Sms(AuthRequest login)
        {
            try
            {
                await _eventLogThresholdService.DoCheck(EventLogTableType.Activity);
                //await _eventLogThresholdService.DoCheck(EventLogTableType.Exception);
                await _eventLogThresholdService.DoCheck(EventLogTableType.Login);
                await _eventLogThresholdService.DoCheck(EventLogTableType.Audits);

                var user = await _userManager.FindByNameAsync(login.UserName);
                var result = await _loginValidator.ValidateAsync(login, options => options.IncludeRuleSets("sms"));
                if (!result.IsValid)
                {
                    _historyLogService.PrepareForInsert($"خطا اعتبارسنجی داده در ورود پیامکی کاربر", EnumFormName.UserLogined, EnumOperation.Post);
                    ViewBag.message = string.Join(" / ", result.Errors.Select(e => e.ErrorMessage).ToArray());
                    InsertUserLoginLog(Strings.Persian.Fields.BadLoginSms, user?.UserName ?? login.UserName, user?.Id ?? "", $"{user?.Name ?? ""} {user?.Family ?? ""}", UserLoginStatus.Failed);
                    return View("Sms", login);
                }

                var SMSExpirationDateTime = TempData["SMSExpirationDateTime"]?.ToString();
                var diffrence = Convert.ToDateTime(SMSExpirationDateTime) - DateTime.UtcNow.AddHours(3.5);
                if (diffrence.TotalSeconds <= 0)
                {
                    ViewBag.message = "کد امنیتی منقضی شده است";
                    InsertUserLoginLog(Strings.Persian.Fields.BadLoginSmsCode, user?.UserName ?? login.UserName, user?.Id ?? "", $"{user?.Name ?? ""} {user?.Family ?? ""}", UserLoginStatus.Failed);
                    return View("SmsChangePassword", login);
                }

                TempData.Keep("smsCodeNew");
                TempData.Keep("SMSExpirationDateTime");
                TempData.Keep("SMSExpirationDateTimeGregorian");
                TempData.Keep("CaptchaCode");

                int.TryParse(TempData["smsCodeNew"]?.ToString(), out int smsCodeValue);
                if (smsCodeValue == 0 || smsCodeValue != login.Sms)
                {
                    ViewBag.message = "کد وارد شده صحیح نمیباشد";
                    login.CaptchaCode = "";
                    InsertUserLoginLog(Strings.Persian.Fields.BadLoginSmsCode, user?.UserName ?? login.UserName, user?.Id ?? "", $"{user?.Name ?? ""} {user?.Family ?? ""}", UserLoginStatus.Failed);
                    return View("Sms", login);
                }

                if (TempData["CaptchaCode"]?.ToString()?.ToLower() != login.CaptchaCode.ToLower())
                {
                    if (_eventLogFilterService.Get().LogBarayeHarGooneTalasheEhrazeHoviateChandGaneyeKarbar)
                        _historyLogService.PrepareForInsert($"ورود ناموفق {login.UserName} بدلیل عدم تطابق کد کپچا", EnumFormName.UserLogined, EnumOperation.Post);

                    ViewBag.message = "کد کپچا وارد شده اشتباه است.";
                    login.CaptchaCode = "";
                    InsertUserLoginLog(Strings.Persian.Fields.BadLoginSmsCaptcha, user?.UserName ?? login.UserName, user?.Id ?? "", $"{user?.Name ?? ""} {user?.Family ?? ""}", UserLoginStatus.Failed);
                    return View("Sms", login);
                }

                var isLoggedIn = await _authenticateService.SmsLogin(login);

                if (!isLoggedIn.Success)
                    InsertUserLoginLog(isLoggedIn.Message, user?.UserName ?? login.UserName, user?.Id ?? "", $"{user?.Name ?? ""} {user?.Family ?? ""}", UserLoginStatus.Failed);
                else
                    InsertUserLoginLog(Strings.Persian.Fields.LogInSms, user?.UserName ?? login.UserName, user?.Id ?? "", $"{user?.Name ?? ""} {user?.Family ?? ""}", UserLoginStatus.Login);

                TempData.Clear();
                TempData["FromLogin"] = true;
                ViewBag.message = isLoggedIn.Message;

                if (isLoggedIn.Success && user?.UserName != null)
                {
                    var sms = await _sendSmsService.SendMessageSmsWithRespondToSuperusers2(user.Family, "کاربر گرامی به نرم افزار شهرسازی آمارد خوش آمدید.", user.Id, 351503);
                    if (sms.StrRetStatus != "با موفقیت ارسال شد")
                        TempData["ValidateMessage"] = "خطا در ارسال پیامک.";
                }

                return isLoggedIn.Success ? RedirectToAction("Index", "Home") : View("Sms");
            }
            catch (Exception e)
            {
                ViewBag.message = "عملیات با خطا مواجه شده است لطفا دوباره تلاش کنید!";
                return View("Sms");
            }
        }

        [ValidateAntiForgeryToken]
        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> Otp(AuthRequest login)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(login.UserName);
                var result = await _loginValidator.ValidateAsync(login, options => options.IncludeRuleSets("OTP"));
                if (!result.IsValid)
                {
                    _historyLogService.PrepareForInsert($"خطا اعتبار سنجی در ورود کاربر {login.UserName} به صورت دو مرحله ای (OTP).", EnumFormName.UserLogined, EnumOperation.Post);
                    ViewBag.message = string.Join(" / ", result.Errors.Select(e => e.ErrorMessage).ToArray());
                    InsertUserLoginLog(Strings.Persian.Fields.BadLoginOtp, user?.UserName ?? login.UserName, user?.Id ?? "", $"{user?.Name ?? ""} {user?.Family ?? ""}", UserLoginStatus.Failed);
                    return View("OTP", login);
                }

                if (login?.CaptchaCode == null)
                {
                    if (_eventLogFilterService.Get().LogBarayeHarGooneTalasheEhrazeHoviateChandGaneyeKarbar)
                        _historyLogService.PrepareForInsert($"ورود ناموفق {login?.UserName} بدلیل عدم تطابق کد کپچا", EnumFormName.UserLogined, EnumOperation.Post);

                    ViewBag.message = "لطفا کد کپچا را وارد کنید!";
                    login.CaptchaCode = "";
                    InsertUserLoginLog(Strings.Persian.Fields.BadLoginOtpCaptcha, user?.UserName ?? login.UserName, user?.Id ?? "", $"{user?.Name ?? ""} {user?.Family ?? ""}", UserLoginStatus.Failed);
                    return View("OTP", login);
                }

                if (TempData["CaptchaCode"]?.ToString()?.ToLower() != login.CaptchaCode.ToLower())
                {
                    if (_eventLogFilterService.Get().LogBarayeHarGooneTalasheEhrazeHoviateChandGaneyeKarbar)
                        _historyLogService.PrepareForInsert($"ورود ناموفق {login.UserName} بدلیل عدم تطابق کد کپچا", EnumFormName.UserLogined, EnumOperation.Post);

                    ViewBag.message = "کد کپچا وارد شده اشتباه است.";
                    login.CaptchaCode = "";
                    InsertUserLoginLog(Strings.Persian.Fields.BadLoginOtpCaptcha, user?.UserName ?? login.UserName, user?.Id ?? "", $"{user?.Name ?? ""} {user?.Family ?? ""}", UserLoginStatus.Failed);
                    return View("OTP", login);
                }

                var isLoggedIn = await _authenticateService.OtpLogin(login);

                if (!isLoggedIn.Success)
                    InsertUserLoginLog(isLoggedIn.Message, user?.UserName ?? login.UserName, user?.Id ?? "", $"{user?.Name ?? ""} {user?.Family ?? ""}", UserLoginStatus.Failed);
                else
                    InsertUserLoginLog(Strings.Persian.Fields.LogInOtp, user?.UserName ?? login.UserName, user?.Id ?? "", $"{user?.Name ?? ""} {user?.Family ?? ""}", UserLoginStatus.Login);

                TempData.Clear();
                TempData["FromLogin"] = true;
                ViewBag.message = isLoggedIn.Message;

                if (isLoggedIn.Success && user?.UserName != null)
                {
                    var sms = await _sendSmsService.SendMessageSmsWithRespondToSuperusers2(user.Family ?? "", "کاربر گرامی به نرم افزار شهرسازی آمارد خوش آمدید.", user.Id ?? "", 351503);
                    if (sms.StrRetStatus != "با موفقیت ارسال شد")
                        TempData["ValidateMessage"] = "خطا در ارسال پیامک.";
                }

                return isLoggedIn.Success ? RedirectToAction("Index", "Home") : View("OTP");
            }
            catch (Exception e)
            {
                ViewBag.message = "عملیات با خطا مواجه شده است لطفا دوباره تلاش کنید!";
                return View("OTP");
            }
        }

        #endregion

        #region Change Password

        [AllowAnonymous]
        public IActionResult ForgetPassword() => View();

        [ValidateAntiForgeryToken]
        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> SmsChangePassword(AuthRequest login)
        {
            await _eventLogThresholdService.DoCheck(EventLogTableType.Activity);
            //await _eventLogThresholdService.DoCheck(EventLogTableType.Exception);
            await _eventLogThresholdService.DoCheck(EventLogTableType.Login);
            await _eventLogThresholdService.DoCheck(EventLogTableType.Audits);

            var result = await _loginValidator.ValidateAsync(login, options => options.IncludeRuleSets("forgetPassword"));
            if (!result.IsValid)
            {
                _historyLogService.PrepareForInsert($"خطا در صحت شماره موبایل", EnumFormName.UserLogined, EnumOperation.Post);
                ViewBag.message = string.Join(" / ", result.Errors.Select(e => e.ErrorMessage).ToArray());
                return View("ForgetPassword");
            }

            var checkUser = await _authenticateService.CheckMobile(login.Mobile);
            if (!checkUser.Success)
            {
                ViewBag.message = checkUser.Message;
                return View("ForgetPassword");
            }

            if (TempData["CaptchaCode"]?.ToString()?.ToLower() != login.CaptchaCode.ToLower())
            {
                ViewBag.message = "کد کپچا وارد شده اشتباه است.";
                return View("ForgetPassword");
            }

            var user = await _authenticateService.GetByUserNameAsync(checkUser.UserName);
            var validSms = await SendSmsMessage(user);
            if (!validSms)
            {
                TempData.Clear();
                ViewBag.message = "خطا در ارسال پیامک.";
                return View("ForgetPassword");
            }

            login.ExpireTime = TempData["SMSExpirationDateTimeGregorian"]?.ToString();
            login.UserName = user.UserName;
            login.CaptchaCode = "";

            return View(login);
        }

        [ValidateAntiForgeryToken]
        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> ChangePassword(AuthRequest login)
        {
            await _eventLogThresholdService.DoCheck(EventLogTableType.Activity);
            //await _eventLogThresholdService.DoCheck(EventLogTableType.Exception);
            await _eventLogThresholdService.DoCheck(EventLogTableType.Login);
            await _eventLogThresholdService.DoCheck(EventLogTableType.Audits);

            var result = await _loginValidator.ValidateAsync(login, options => options.IncludeRuleSets("sms"));
            if (!result.IsValid)
            {
                _historyLogService.PrepareForInsert($"خطا در تغییر رمز عبور کاربر {login.UserName} به صورت پیامکی", EnumFormName.UserLogined, EnumOperation.Post);
                ViewBag.message = string.Join(" / ", result.Errors.Select(e => e.ErrorMessage).ToArray());
                return View("SmsChangePassword", login);
            }

            var SMSExpirationDateTime = TempData["SMSExpirationDateTime"]?.ToString();
            var diffrence = Convert.ToDateTime(SMSExpirationDateTime) - DateTime.UtcNow.AddHours(3.5);
            if (diffrence.TotalSeconds <= 0)
            {
                ViewBag.message = "کد امنیتی منقضی شده است";
                return View("SmsChangePassword", login);
            }

            TempData.Keep("smsCodeNew");
            TempData.Keep("SMSExpirationDateTime");
            TempData.Keep("SMSExpirationDateTimeGregorian");
            TempData.Keep("CaptchaCode");

            int.TryParse(TempData["smsCodeNew"]?.ToString(), out int smsCodeValue);
            if (smsCodeValue == 0 || smsCodeValue != login.Sms)
            {
                ViewBag.message = "کد وارد شده صحیح نمی باشد";
                login.CaptchaCode = "";
                return View("SmsChangePassword");
            }

            if (TempData["CaptchaCode"]?.ToString()?.ToLower() != login.CaptchaCode.ToLower())
            {
                ViewBag.message = "کد کپچا وارد شده اشتباه است.";
                login.CaptchaCode = "";
                return View("SmsChangePassword", login);
            }

            var changePass = new ChangePasswordDto
            {
                UserName = login.UserName
            };

            return View(changePass);
        }

        [ValidateAntiForgeryToken]
        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> ChangePasswordSubmit(ChangePasswordDto command)
        {
            await _eventLogThresholdService.DoCheck(EventLogTableType.Activity);
            //await _eventLogThresholdService.DoCheck(EventLogTableType.Exception);
            await _eventLogThresholdService.DoCheck(EventLogTableType.Login);
            await _eventLogThresholdService.DoCheck(EventLogTableType.Audits);
            command.OldPassword = command.NewPassword;
            ValidationResult validate = _changePasswordValidator.Validate(command);
            if (!validate.IsValid)
            {
                _historyLogService.PrepareForInsert($"خطا اعتبار سنجی تغییر رمز عبور کاربر {command.UserName}", EnumFormName.UserLogined, EnumOperation.Post);
                ViewBag.message = string.Join(" / ", validate.Errors.Select(e => e.ErrorMessage).ToArray());
                return View("ChangePassword");
            }

            var resultPassword = await _ExtensionUser.CheckPasswordValidation(command.NewPassword);
            if (!resultPassword.IsOk)
            {
                ViewBag.message = resultPassword.Message;
                return View("ChangePassword", command);
            }

            var user = await _authenticateService.GetByUserNameAsync(command.UserName);

            var validation = ValidationPassword.IsValidPassword(command.NewPassword, user.PhoneNumber, "");
            if (!validation.Success)
            {
                ViewBag.message = validation.Message;
                return View("ChangePassword", command);
            }

            // 1. Check if the old password is correct
            var isOldPasswordCorrect = await _userManager.CheckPasswordAsync(user, command.NewPassword);
            if (isOldPasswordCorrect)
            {
                ViewBag.message = ValidationMessage.RepeatPassword2();
                return View("ChangePassword", command);
            }

            //await _userManager.RemovePasswordAsync(user);
            //var result = await _userManager.AddPasswordAsync(user, command.NewPassword);
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            // تغییر رمز عبور با توکن
            var result = await _userManager.ResetPasswordAsync(user, token, command.NewPassword);
            if (result.Succeeded)
            {
                await _userManager.UpdateSecurityStampAsync(user);

                var sms = await _sendSmsService.SendMessageSmsWithRespondToSuperusers2(user?.Family ?? " ", "کاربر گرامی رمز عبور شما با موفقیت تغییر کرد.", user?.Id ?? "", 351506);
                if (sms.StrRetStatus != "با موفقیت ارسال شد")
                    ViewBag.message = "خطا در ارسال پیامک.";

                TempData["ChangePasswordSuccess"] = true;
                return RedirectToAction("Index");
            }

            var message = string.Join(" / ", result.Errors.Select(e => e.Description).ToArray());
            ViewBag.message = message;
            return View("ChangePassword", command);
        }

        #endregion

        #region Captcha

        [AllowAnonymous]
        public IActionResult GenerateCaptcha()
        {
            string captchaText = GenerateRandomText(5);
            var captcha = new CaptchaImage(captchaText, 200, 40);
            Bitmap captchaImage = captcha.Image;
            var ms = new MemoryStream();
            captchaImage.Save(ms, ImageFormat.Png);
            TempData["CaptchaCode"] = captcha.Text;
            return File(ms.ToArray(), "image/png");
        }

        private string GenerateRandomText(int length)
        {
            const string chars = "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        #endregion

        #region Sms

        private string PrepareMobileNumbers(string phoneNumber)
        {
            if (phoneNumber.StartsWith("0") && phoneNumber.Length == 11)
                phoneNumber = "98" + phoneNumber[1..];
            else
                phoneNumber = "98" + phoneNumber;

            return phoneNumber;
        }

        private async Task<bool> SendSmsMessage(CostumIdentityUser user)
        {
            var random = new Random();
            var code = random.Next(100000, 999999);

            var message = $"نرم افزار یکپارچه شهرسازی آمارد \n \n ارسال کد احراز هویت";
            var expirationTime = DateTime.UtcNow.AddMinutes(212);
            var expirationTimeGregorian = expirationTime.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

            var mobileNumbers = PrepareMobileNumbers(user.PhoneNumber ?? "");
            var sms = await _melipayamakSmsService.SendSms(code.ToString(), message, mobileNumbers, 342391);

            if (sms.StrRetStatus != "با موفقیت ارسال شد")
                return false;

            TempData["smsCodeNew"] = code.ToString();
            TempData["SMSExpirationDateTime"] = expirationTime;
            TempData["SMSExpirationDateTimeGregorian"] = expirationTimeGregorian;

            //return sms.Any();
            return true;
        }

        [ValidateAntiForgeryToken]
        [HttpPost, AllowAnonymous]
        [Route("Login/ResendSmsCode")]
        public async Task<IActionResult> ResendSmsCode(string userName)
        {
            var SMSExpirationDateTime = TempData["SMSExpirationDateTime"]?.ToString();
            var diffrence = Convert.ToDateTime(SMSExpirationDateTime) - DateTime.UtcNow.AddHours(3.5);
            if (diffrence.TotalSeconds > 0)
            {
                TempData.Keep("smsCodeNew");
                TempData.Keep("SMSExpirationDateTime");
                TempData.Keep("SMSExpirationDateTimeGregorian");
                return new JsonResult(new { success = false, error = "کد امنیتی منقضی نشده است." });
            }

            var user = await _authenticateService.GetByUserNameAsync(userName);
            if (user == null || string.IsNullOrWhiteSpace(user.PhoneNumber))
                return new JsonResult(new { success = false, error = "کاربر یافت نشد." });

            var validSms = await SendSmsMessage(user);
            if (!validSms)
                return new JsonResult(new { success = false, error = "خطا در ارسال پیامک." });

            var expireTime = ((DateTime)TempData["SMSExpirationDateTime"]).ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

            TempData.Keep("smsCodeNew");
            TempData.Keep("SMSExpirationDateTime");
            TempData.Keep("SMSExpirationDateTimeGregorian");

            return new JsonResult(new { success = true, expireTime = expireTime });
        }

        #endregion

        #region Logout

        public async Task<IActionResult> Logout(bool sessionExpired = false, bool AccessDenied = false, bool hashNotValid = false, bool roleRestriction = false,
            bool invalidHashUser = false, bool invalidHashRole = false, bool userDeactive = false)
        {
            var user = _authenticateService.GetCurrentUser();


            if (!sessionExpired && TempData["sessionExpired"]?.ToString() == "Expired")
            {
                sessionExpired = true;
                TempData.Remove("sessionExpired");
            }

            if (_eventLogFilterService.Get().MustLoginBeLogged)
            {
                var badLogin = new UserLogined
                {
                    UserName = user.UserName,
                    FullName = $"{user.Name} {user.Family}",
                    LogoutDatetime = DateTime.UtcNow.AddHours(3.5),
                    LoginDateTime = null,
                    UserCode = user.Id,
                    UserAgent = _browserService.GetBrowserDetails(),
                    Ip = _ipService.GetIp(),
                    Status = (byte)UserLoginStatus.Logout
                };


                if (user.UserName != null && sessionExpired)
                    badLogin.Method = Strings.Persian.Fields.ForceLogout;

                else if (user.UserName != null && AccessDenied)
                    badLogin.Method = Strings.Persian.Fields.AccessDeniedLogOut;

                else if (user.UserName != null && hashNotValid)
                    badLogin.Method = Strings.Persian.Fields.DataIntegrity;

                else if (user.UserName != null && roleRestriction)
                    badLogin.Method = Strings.Persian.Fields.roleRestriction;

                else if (user.UserName != null && invalidHashUser)
                    badLogin.Method = Strings.Persian.Fields.InvalidHashUser;

                else if (user.UserName != null && invalidHashRole)
                    badLogin.Method = Strings.Persian.Fields.InvalidHashRole;

                else if (user.UserName != null && userDeactive)
                    badLogin.Method = Strings.Persian.Fields.userDeactive;

                else if (user.UserName != null)
                    badLogin.Method = Strings.Persian.Fields.LogOut;

                _userLoginedService.Insert(badLogin);
            }

            await _authenticateService.Logout(user.Id);
            //await _sendSmsService.SendMessageSmsWithRespondToSuperusers("کاربر گرامی خروج شما از نرم افزار یکپارچه شهرسازی آمارد با موفقیت انجام شد.", user.Id);
            return RedirectToAction("Index");
        }
        #endregion

        private void InsertUserLoginLog(string method, string userName, string userId, string fullName, UserLoginStatus status)
        {
            var badLogin = new UserLogined();

            if (method == Strings.Persian.Fields.LogIn)
            {
                if (_eventLogFilterService.Get().MustLoginBeLogged)
                    badLogin = new UserLogined
                    {
                        UserName = userName,
                        FullName = fullName,
                        LogoutDatetime = null,
                        LoginDateTime = DateTime.UtcNow.AddHours(3.5),
                        UserCode = userId,
                        Method = method,
                        UserAgent = _browserService.GetBrowserDetails(),
                        Ip = _ipService.GetIp(),
                        Status = (byte)status
                    };
                else
                    return;
            }
            else
            {
                if (_eventLogFilterService.Get().LogBarayeHarGooneTalasheEhrazeHoviateChandGaneyeKarbar)
                    badLogin = new UserLogined
                    {
                        UserName = userName,
                        FullName = fullName,
                        LogoutDatetime = null,
                        LoginDateTime = DateTime.UtcNow.AddHours(3.5),
                        UserCode = userId,
                        Method = method,
                        UserAgent = _browserService.GetBrowserDetails(),
                        Ip = _ipService.GetIp(),
                        Status = (byte)status
                    };
                else
                    return;
            }

            _userLoginedService.Insert(badLogin);
        }
    }
}
