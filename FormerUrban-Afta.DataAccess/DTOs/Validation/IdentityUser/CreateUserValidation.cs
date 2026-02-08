using FormerUrban_Afta.DataAccess.DTOs.IdentityUser;
using FormerUrban_Afta.DataAccess.Services;

namespace FormerUrban_Afta.DataAccess.DTOs.Validation.IdentityUser
{
    public class CreateUserValidation : AbstractValidator<CreateCostumIdentityUserDto>
    {
        public CreateUserValidation()
        {
            RuleFor(x => x.OldUserName)
                .Must(ValidatorService.SanitizeAndValidateInput).WithMessage(ValidationMessage.SanitizeInput("نام کاربری قدیم"))
                .MinimumLength(3).WithMessage(ValidationMessage.MinLength("نام کاربری قدیم", 3))
                .MaximumLength(200).WithMessage(ValidationMessage.MaxLength("نام کاربریی قدیم", 200))
                .When(x => !string.IsNullOrWhiteSpace(x.UserName));

            RuleFor(x => x.Name)
                .MaximumLength(200).WithMessage(ValidationMessage.MaxLength("نام", 200))
                .Must(ValidatorService.SanitizeAndValidateInput).WithMessage(ValidationMessage.SanitizeInput("نام"))
                .Must(ValidatorService.IsPersianLetters).WithMessage(ValidationMessage.IsPersianLetters("نام"))
                .When(x => !string.IsNullOrWhiteSpace(x.Name))
                .NotEmpty().WithMessage(ValidationMessage.Required("نام"));

            RuleFor(x => x.Family)
                .MaximumLength(200).WithMessage(ValidationMessage.MaxLength("نام خانوادگی", 200))
                .Must(ValidatorService.SanitizeAndValidateInput).WithMessage(ValidationMessage.SanitizeInput("نام خانوادگی"))
                .Must(ValidatorService.IsPersianLetters).WithMessage(ValidationMessage.IsPersianLetters("نام خانوادگی"))
                .When(x => !string.IsNullOrWhiteSpace(x.Family))
                .NotEmpty().WithMessage(ValidationMessage.Required("نام خانوادگی"));

            RuleFor(x => x.UserName)
                .Must(ValidatorService.SanitizeAndValidateInput).WithMessage(ValidationMessage.SanitizeInput("نام کاربری"))
                .When(x => !string.IsNullOrWhiteSpace(x.UserName))
                .NotEmpty().WithMessage(ValidationMessage.Required("نام کاربری"))
                .MinimumLength(3).WithMessage(ValidationMessage.MinLength("نام کاربری", 3))
                .MaximumLength(200).WithMessage(ValidationMessage.MaxLength("نام کاربری", 200));

            RuleFor(x => x.PasswordHash)
                .Must(ValidatorService.SanitizeAndValidateInput)
                .WithMessage(ValidationMessage.SanitizeInput("رمز عبور"))
                .When(x => !string.IsNullOrWhiteSpace(x.PasswordHash))
                .NotEmpty().WithMessage(ValidationMessage.Required("رمز عبور"))
                .When(x => string.IsNullOrWhiteSpace(x.OldUserName));

            RuleFor(x => x.RepeatPassword)
                .Must(ValidatorService.SanitizeAndValidateInput).WithMessage(ValidationMessage.SanitizeInput("تکرار رمز عبور"))
                .When(x => !string.IsNullOrWhiteSpace(x.RepeatPassword))
                .NotEmpty().WithMessage(ValidationMessage.Required("تکرار رمز عبور"))
                .When(x => string.IsNullOrWhiteSpace(x.OldUserName))
                .Equal(x => x.PasswordHash).WithMessage(ValidationMessage.RepeatPassword());

            RuleFor(x => x.Role)
                .NotNull().WithMessage("لطفا یک نقش را انتخاب کنید!");

            RuleFor(x => x.PhoneNumber)
                .Must(ValidatorService.IsValidMobileNumber).WithMessage(ValidationMessage.IsValidMobileNumber())
                .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber))
                .NotEmpty().WithMessage(ValidationMessage.Required("شماره همراه"));

            RuleFor(x => x.Email)
                .Must(ValidatorService.SanitizeAndValidateInput).WithMessage(ValidationMessage.SanitizeInput("ایمیل"))
                .MaximumLength(320).WithMessage(ValidationMessage.MaxLength("ایمیل", 320))
                .EmailAddress().WithMessage(ValidationMessage.IsValidEmail())
                .When(x => !string.IsNullOrWhiteSpace(x.Email));
        }
    }
}
