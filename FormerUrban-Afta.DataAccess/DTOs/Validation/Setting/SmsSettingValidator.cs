using FormerUrban_Afta.DataAccess.Services;

namespace FormerUrban_Afta.DataAccess.DTOs.Validation.Setting;

public class SmsSettingValidator : AbstractValidator<SmsSettingDto>
{
    public SmsSettingValidator()
    {
        RuleFor(x => x.sms_user)
            .MinimumLength(3).WithMessage(ValidationMessage.MinLength("نام کاربری", 3))
            .MaximumLength(30).WithMessage(ValidationMessage.MaxLength("نام کاربری", 30))
            .Must(ValidatorService.SanitizeAndValidateInput).WithMessage(ValidationMessage.SanitizeInput("نام کاربری"))
            .Must(ValidatorService.IsAlphanumeric).WithMessage(ValidationMessage.IsAlphanumeric("نام کاربری"))
            .When(x => !string.IsNullOrWhiteSpace(x.sms_user))
            .NotEmpty().WithMessage(ValidationMessage.Required("نام کاربری"));

        RuleFor(x => x.sms_pass)
            .MinimumLength(3).WithMessage(ValidationMessage.MinLength("رمز عبور", 3))
            .MaximumLength(128).WithMessage(ValidationMessage.MaxLength("رمز عبور", 128))
            .Must(ValidatorService.SanitizeAndValidateInput).WithMessage(ValidationMessage.SanitizeInput("رمز عبور"))
            //.Must(ValidatorService.IsAlphanumeric).WithMessage(ValidationMessage.IsAlphanumeric("رمز عبور"))
            .When(x => !string.IsNullOrWhiteSpace(x.sms_user))
            .NotEmpty().WithMessage(ValidationMessage.Required("رمز عبور"));

        //RuleFor(x => x.SmsAuthorizationtime)
        //    .GreaterThanOrEqualTo(60).WithMessage("حداقل زمان زمان انقضای کد پیامک شده 60 ثانیه است.")
        //    .LessThanOrEqualTo(3600).WithMessage("حداکثر زمان زمان انقضای کد پیامک شده 3600 ثانیه است.")
        //    .NotEmpty().WithMessage(ValidationMessage.Required("زمان انقضای کد پیامک شده"));
    }
}
