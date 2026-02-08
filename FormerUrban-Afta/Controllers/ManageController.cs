using FormerUrban_Afta.Attributes;
using FormerUrban_Afta.DataAccess.DTOs.Login;
using Microsoft.AspNetCore.Authorization;
using QRCoder;
using System.Text;
using System.Text.Encodings.Web;

namespace FormerUrban_Afta.Controllers;

[Authorize]
public class ManageController : AmardBaseController
{
    private readonly UserManager<CostumIdentityUser> _userManager;
    private readonly SignInManager<CostumIdentityUser> _signInManager;
    private readonly UrlEncoder _urlEncoder;
    private readonly IAuthService _authService;
    private readonly ISendSmsService _sendSmsService;
    private readonly IHistoryLogService _historyLogService;

    public ManageController(UserManager<CostumIdentityUser> userManager, UrlEncoder urlEncoder,
        SignInManager<CostumIdentityUser> signInManager, IAuthService authService,
        ISendSmsService sendSmsService, IHistoryLogService historyLogService)
    {
        _userManager = userManager;
        _urlEncoder = urlEncoder;
        _signInManager = signInManager;
        _authService = authService;
        _sendSmsService = sendSmsService;
        _historyLogService = historyLogService;
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    [CheckUserAccess(permissionCode: "EnableAuthenticator", type: EnumOperation.Get, table: EnumFormName.AspNetUsers, section: "فعال سازی تایید دو مرحله ای")]
    public async Task<IActionResult> EnableAuthenticator(string userName)
    {
        if (string.IsNullOrWhiteSpace(userName))
            return RedirectToAction("index", "User", new { area = "IdentityUser" });

        var user = await _authService.GetByUserNameAsync(userName);
        var key = await _userManager.GetAuthenticatorKeyAsync(user);

        if (string.IsNullOrEmpty(key))
        {
            await _userManager.ResetAuthenticatorKeyAsync(user);
            key = await _userManager.GetAuthenticatorKeyAsync(user);
        }

        var email = await _userManager.GetEmailAsync(user);
        var authenticatorUri = GenerateQrCodeUri("FormerUrban", email ?? $"{user.PhoneNumber}", key);

        // Generate QR Code
        var qrCodeImage = GenerateQrCodeImage(authenticatorUri);

        var model = new EnableAuthenticatorDto
        {
            SharedKey = FormatKey(key),
            AuthenticatorUri = authenticatorUri,
            QrCodeImage = qrCodeImage // Pass the QR code image to the view
        };

        return PartialView(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [CheckUserAccess(permissionCode: "EnableAuthenticator", type: EnumOperation.Post, table: EnumFormName.AspNetUsers, section: "فعال سازی تایید دو مرحله ای")]
    public async Task<IActionResult> EnableAuthenticatorSubmit(EnableAuthenticatorDto model)
    {
        if (string.IsNullOrWhiteSpace(model.UserName))
        {
            _historyLogService.PrepareForInsert(description: "خطا در فعال سازی احراز هویت دو مرحله ای:کاربر یافت نشد",
                EnumFormName.AspNetUsers, EnumOperation.Post);
            return new JsonResult(new { success = false, message = "کاربر مورد نظر یافت نشد!" });
        }

        if (!ValidatorService.IsDigitsOnly(model.Code))
        {
            _historyLogService.PrepareForInsert(description: "خطا در فعال سازی احراز هویت دو مرحله ای:کد نامعتبر است",
                EnumFormName.AspNetUsers, EnumOperation.Post);
            return new JsonResult(new { success = false, message = "لطفا از اعداد استفاده کنید." });
        }

        if (model.Code.Length != 6)
        {
            _historyLogService.PrepareForInsert(description: "خطا در فعال سازی احراز هویت دو مرحله ای:کد نامعتبر است",
                EnumFormName.AspNetUsers, EnumOperation.Post);
            return new JsonResult(new { success = false, message = "طول کد تایید باید 6 رقم باشد!" });
        }

        var user = await _authService.GetByUserNameAsync(model.UserName);
        if (user == null)
        {
            _historyLogService.PrepareForInsert(description: "خطا در فعال سازی احراز هویت دو مرحله ای:کاربر یافت نشد",
                EnumFormName.AspNetUsers, EnumOperation.Post);
            return new JsonResult(new { success = false, message = "کاربر مورد نظر یافت نشد!" });
        }

        var isValid = await _userManager.VerifyTwoFactorTokenAsync(user, _userManager.Options.Tokens.AuthenticatorTokenProvider, model.Code.ToString());
        if (!isValid)
        {
            _historyLogService.PrepareForInsert(description: "خطا در فعال سازی احراز هویت دو مرحله ای:کد وارد شده مورد تایید نمی باشد",
                EnumFormName.AspNetUsers, EnumOperation.Post);
            return new JsonResult(new { success = false, message = "عملیات انجام نشد لطفا مجددا تلاش کنید!" });
        }

        await _userManager.SetTwoFactorEnabledAsync(user, true);
        var sms = await _sendSmsService.SendMessageSmsWithRespondToSuperusers2("فعال", "کاربر گرامی تایید دومرحله شما با موفقیت فعال شد.", user.Id, 351501);
        if (sms.StrRetStatus != "با موفقیت ارسال شد")
            TempData["ValidateMessage"] = "خطا در ارسال پیامک.";
        TempData["SuccessMessage"] = "تایید دو مرحله ای کاربر با موفقیت فعال شد.";

        _historyLogService.PrepareForInsert(description: $"فعال سازی ورود دومرحله ای کاربر {user.UserName}", EnumFormName.AspNetUsers, EnumOperation.Post);

        return new JsonResult(new { success = true, message = "" });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [CheckUserAccess(permissionCode: "EnableAuthenticator", type: EnumOperation.Get, table: EnumFormName.AspNetUsers, section: "غیر فعال سازی تایید دو مرحله ای")]
    public IActionResult DisableAuthenticator(string userName)
    {
        if (string.IsNullOrWhiteSpace(userName))
            return RedirectToAction("index", "User", new { area = "IdentityUser" });

        var model = new EnableAuthenticatorDto
        {
            UserName = userName
        };
        return PartialView(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [CheckUserAccess(permissionCode: "EnableAuthenticator", type: EnumOperation.Post, table: EnumFormName.AspNetUsers, section: "غیر فعال سازی تایید دو مرحله ای")]
    public async Task<IActionResult> DisableAuthenticatorSubmit(EnableAuthenticatorDto model)
    {
        if (string.IsNullOrWhiteSpace(model.UserName))
        {
            _historyLogService.PrepareForInsert(description: "خطا در غیرفعال سازی احراز هویت دو مرحله ای:کاربر یافت نشد",
                EnumFormName.AspNetUsers, EnumOperation.Post);
            return new JsonResult(new { success = false, message = "کاربر مورد نظر یافت نشد!" });
        }

        if (!ValidatorService.IsDigitsOnly(model.Code))
        {
            _historyLogService.PrepareForInsert(description: "خطا در غیر فعال سازی احراز هویت دو مرحله ای:کد نامعتبر است",
                EnumFormName.AspNetUsers, EnumOperation.Post);
            return new JsonResult(new { success = false, message = "لطفا از اعداد استفاده کنید." });
        }

        if (model.Code.ToString().Length != 6)
        {
            _historyLogService.PrepareForInsert(description: "خطا در غیرفعال سازی احراز هویت دو مرحله ای:کد نامعتبر است",
                EnumFormName.AspNetUsers, EnumOperation.Post);
            return new JsonResult(new { success = false, message = "طول کد تایید باید 6 رقم باشد!" });
        }

        var user = await _authService.GetByUserNameAsync(model.UserName);
        if (user == null)
        {
            _historyLogService.PrepareForInsert(description: "خطا در غیرفعال سازی احراز هویت دو مرحله ای:کاربر یافت نشد",
                EnumFormName.AspNetUsers, EnumOperation.Post);
            return new JsonResult(new { success = false, message = "کاربر مورد نظر یافت نشد!" });
        }

        var isValid = await _userManager.VerifyTwoFactorTokenAsync(user, _userManager.Options.Tokens.AuthenticatorTokenProvider, model.Code.ToString());
        if (!isValid)
        {
            _historyLogService.PrepareForInsert(description: "خطا در غیرفعال سازی احراز هویت دو مرحله ای:کد وارد شده صحیح نمی باشد",
                EnumFormName.AspNetUsers, EnumOperation.Post);
            return new JsonResult(new { success = false, message = "عملیات انجام نشد لطفا مجددا تلاش کنید!" });
        }

        await _userManager.SetTwoFactorEnabledAsync(user, false);
        var sms = await _sendSmsService.SendMessageSmsWithRespondToSuperusers2("غیر فعال", "کاربر گرامی تایید دومرحله شما با موفقیت غیر فعال شد.", user.Id, 351501);

        if (sms.StrRetStatus != "با موفقیت ارسال شد")
            TempData["ValidateMessage"] = "خطا در ارسال پیامک.";

        TempData["SuccessMessage"] = "تایید دو مرحله ای کابر با موفقیت غیر فعال شد.";

        _historyLogService.PrepareForInsert(description: $"غیر فعال سازی ورود دومرحله ای کاربر {user.UserName}", EnumFormName.AspNetUsers, EnumOperation.Post);

        return new JsonResult(new { success = true, message = "" });
    }


    [HttpGet]
    public async Task<IActionResult> TwoFactorAuthentication()
    {
        var user = await _userManager.GetUserAsync(User);

        var model = new TwoFactorAuthenticationDto
        {
            HasAuthenticator = await _userManager.GetAuthenticatorKeyAsync(user) != null,
            Is2faEnabled = await _userManager.GetTwoFactorEnabledAsync(user),
            IsMachineRemembered = await _signInManager.IsTwoFactorClientRememberedAsync(user),
            RecoveryCodesLeft = await _userManager.CountRecoveryCodesAsync(user)
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> ForgetTwoFactorClient()
    {
        var user = await _userManager.GetUserAsync(User);
        await _signInManager.ForgetTwoFactorClientAsync();
        return RedirectToAction(nameof(TwoFactorAuthentication));
    }

    [HttpPost]
    public async Task<IActionResult> ResetAuthenticator()
    {
        var user = await _userManager.GetUserAsync(User);

        await _userManager.SetTwoFactorEnabledAsync(user, false);
        await _userManager.ResetAuthenticatorKeyAsync(user);

        return RedirectToAction(nameof(EnableAuthenticator));
    }

    [HttpPost]
    public async Task<IActionResult> GenerateRecoveryCodes()
    {
        var user = await _userManager.GetUserAsync(User);

        if (!await _userManager.GetTwoFactorEnabledAsync(user))
        {
            throw new InvalidOperationException("Cannot generate recovery codes for user without 2FA enabled.");
        }

        var recoveryCodes = await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);
        TempData["RecoveryCodes"] = recoveryCodes.ToArray();

        return RedirectToAction(nameof(ShowRecoveryCodes));
    }

    [HttpGet]
    public IActionResult ShowRecoveryCodes()
    {
        var recoveryCodes = TempData["RecoveryCodes"] as string[];

        if (recoveryCodes == null || recoveryCodes.Length == 0)
        {
            // if no recovery codes, redirect back to 2FA page
            return RedirectToAction(nameof(TwoFactorAuthentication));
        }

        return View();
    }

    [HttpGet]
    public IActionResult ReAuthenticate(string returnUrl = null)
    {
        return View(new ReAuthenticateDto { ReturnUrl = returnUrl });
    }

    [HttpPost]
    public async Task<IActionResult> ReAuthenticate(ReAuthenticateDto model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _userManager.GetUserAsync(User);

        if (user == null)
        {
            throw new InvalidOperationException("Unable to load user.");
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, lockoutOnFailure: true);
        if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, "Incorrect password.");
            return View(model);
        }

        await _signInManager.SignInAsync(user, isPersistent: false);
        if (!string.IsNullOrEmpty(model.ReturnUrl))
        {
            return Redirect(model.ReturnUrl);
        }

        return RedirectToAction(nameof(Index), "Home");
    }


    private string GenerateQrCodeUri(string appName, string email, string unformattedKey)
    {
        return string.Format(
            "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6",
            _urlEncoder.Encode(appName),
            _urlEncoder.Encode(email),
            unformattedKey);
    }

    private string FormatKey(string unformattedKey)
    {
        // For better readability
        var result = new StringBuilder();
        int currentPosition = 0;
        while (currentPosition + 4 < unformattedKey.Length)
        {
            result.Append(unformattedKey.Substring(currentPosition, 4)).Append(" ");
            currentPosition += 4;
        }
        if (currentPosition < unformattedKey.Length)
        {
            result.Append(unformattedKey.Substring(currentPosition));
        }
        return result.ToString().ToLowerInvariant();
    }

    private byte[] GenerateQrCodeImage(string authenticatorUri)
    {
        if (string.IsNullOrEmpty(authenticatorUri))
            throw new ArgumentException("Authenticator URI cannot be null or empty.");

        using (var qrCodeGenerator = new QRCodeGenerator())
        {
            var qrCodeData = qrCodeGenerator.CreateQrCode(authenticatorUri, QRCodeGenerator.ECCLevel.Q);
            using (var qrCode = new PngByteQRCode(qrCodeData)) // Use PngByteQRCode instead of QRCode
            {
                return qrCode.GetGraphic(3); // Returns byte array directly
            }
        }
    }

}
