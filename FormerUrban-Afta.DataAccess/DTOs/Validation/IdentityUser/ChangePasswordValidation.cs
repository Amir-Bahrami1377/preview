using FormerUrban_Afta.DataAccess.DTOs.IdentityUser;
using FormerUrban_Afta.DataAccess.Services;

namespace FormerUrban_Afta.DataAccess.DTOs.Validation.IdentityUser;

public class ChangePasswordValidation : AbstractValidator<ChangePasswordDto>
{
    public ChangePasswordValidation()
    {
        RuleFor(x => x.OldPassword)
            .Must(ValidatorService.SanitizeAndValidateInput)
            .WithMessage(ValidationMessage.SanitizeInput("رمز عبور قدیم"))
            .When(x => !string.IsNullOrWhiteSpace(x.UserName))
            .NotEmpty().WithMessage(ValidationMessage.Required("رمز عبور قدیم"));

        RuleFor(x => x.NewPassword)
            .Must(ValidatorService.SanitizeAndValidateInput)
            .WithMessage(ValidationMessage.SanitizeInput("رمز عبور جدید"))
            .When(x => !string.IsNullOrWhiteSpace(x.NewPassword))
            .NotEmpty().WithMessage(ValidationMessage.Required("رمز عبور جدید"));

        RuleFor(x => x.RepeatPassword)
            .Must(ValidatorService.SanitizeAndValidateInput).WithMessage(ValidationMessage.SanitizeInput("تکرار رمز عبور جدید"))
            .When(x => !string.IsNullOrWhiteSpace(x.UserName))
            .NotEmpty().WithMessage(ValidationMessage.Required("تکرار رمز عبور جدید"))
            .When(x => string.IsNullOrWhiteSpace(x.RepeatPassword))
            .Equal(x => x.NewPassword).WithMessage(ValidationMessage.RepeatPassword());
    }
}

