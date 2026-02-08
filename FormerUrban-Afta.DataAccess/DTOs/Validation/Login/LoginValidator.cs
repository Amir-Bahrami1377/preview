using FormerUrban_Afta.DataAccess.DTOs.Login;
using FormerUrban_Afta.DataAccess.Services;

namespace FormerUrban_Afta.DataAccess.DTOs.Validation.Login
{
    public class LoginValidator : AbstractValidator<AuthRequest>
    {
        public LoginValidator()
        {
            RuleSet("login", () =>
            {
                RuleFor(x => x.UserName)
                    .Must(ValidatorService.SanitizeAndValidateInput).WithMessage(ValidationMessage.SanitizeInput("نام کاربری"))
                    .When(x => !string.IsNullOrWhiteSpace(x.UserName))
                    .NotEmpty().WithMessage(ValidationMessage.Required("نام کاربری"))
                    .MinimumLength(3).WithMessage(ValidationMessage.MinLength("نام کاربری", 3))
                    .MaximumLength(200).WithMessage(ValidationMessage.MaxLength("نام کاربری", 200));

                RuleFor(x => x.Password)
                    .Must(ValidatorService.SanitizeAndValidateInput).WithMessage(ValidationMessage.SanitizeInput("رمز عبور"))
                    .When(x => !string.IsNullOrWhiteSpace(x.Password))
                    .NotEmpty().WithMessage(ValidationMessage.Required("رمز عبور"))
                    .MinimumLength(12).WithMessage(ValidationMessage.MinLength("رمز عبور", 12))
                    .MaximumLength(128).WithMessage(ValidationMessage.MaxLength("رمز عبور", 128));

            });

            RuleSet("Otp", () =>
            {
                RuleFor(x => x.Code)
                    .Must(ValidatorService.SanitizeAndValidateInput).WithMessage(ValidationMessage.SanitizeInput("کد OTP"))
                    .Must(ValidatorService.IsDigitsOnly).WithMessage(ValidationMessage.OnlyDigits("کد OTP"))
                    .When(x => !string.IsNullOrWhiteSpace(x.Code))
                    .NotEmpty().WithMessage(ValidationMessage.Required("کد OTP"))
                    .Length(6).WithMessage(ValidationMessage.Equal("کد OTP", "6 کارکتر"));

                RuleFor(x => x.CaptchaCode)
                    .Must(ValidatorService.SanitizeAndValidateInput).WithMessage(ValidationMessage.SanitizeInput("کد کپچا"))
                    .When(x => !string.IsNullOrWhiteSpace(x.CaptchaCode))
                    .NotEmpty().WithMessage(ValidationMessage.Required("کد کپچا"))
                    .Length(5).WithMessage(ValidationMessage.Equal("کد کپچا", "5 کارکتر"));
            });

            RuleSet("forgetPassword", () =>
            {
                RuleFor(x => x.Mobile)
                    .Must(ValidatorService.IsValidMobileNumber).WithMessage(ValidationMessage.IsValidMobileNumber())
                    .When(x => !string.IsNullOrWhiteSpace(x.Mobile))
                    .NotEmpty().WithMessage(ValidationMessage.Required("شماره همراه"));

                RuleFor(x => x.CaptchaCode)
                    .Must(ValidatorService.SanitizeAndValidateInput).WithMessage(ValidationMessage.SanitizeInput("کد کپچا"))
                    .When(x => !string.IsNullOrWhiteSpace(x.CaptchaCode))
                    .NotEmpty().WithMessage(ValidationMessage.Required("کد کپچا"))
                    .Length(5).WithMessage(ValidationMessage.Equal("کد کپچا", "5 کارکتر"));
            });

            RuleSet("sms", () =>
            {
                RuleFor(x => x.Sms)
                    .Must(Sms => Sms >= 100000 && Sms <= 999999).WithMessage(ValidationMessage.Equal("کد پیامک شده", "6 رقم"))
                    .When(x => x.Code != null)
                    .NotEmpty().WithMessage(ValidationMessage.Required("کد پیامک شده"));

                RuleFor(x => x.CaptchaCode)
                    .Must(ValidatorService.SanitizeAndValidateInput).WithMessage(ValidationMessage.SanitizeInput("کد کپچا"))
                    .When(x => !string.IsNullOrWhiteSpace(x.CaptchaCode))
                    .NotEmpty().WithMessage(ValidationMessage.Required("کد کپچا"))
                    .Length(5).WithMessage(ValidationMessage.Equal("کد کپچا", "5 کارکتر"));
            });
        }
    }
}
