using FormerUrban_Afta.DataAccess.DTOs.IdentityUser;
using FormerUrban_Afta.DataAccess.Services;

namespace FormerUrban_Afta.DataAccess.DTOs.Validation.Setting
{
    public class UserLoginedValidation : AbstractValidator<UserLoginedSearchDto>
    {
        public UserLoginedValidation()
        {
            RuleFor(x => x.FromDateTime)
                .Must(ValidatorService.IsValidPersianDate)
                .WithMessage(ValidationMessage.IsValidPersianDate("تاریخ شروع"));

            RuleFor(x => x.ToDateTime)
                .Must(ValidatorService.IsValidPersianDate)
                .WithMessage(ValidationMessage.IsValidPersianDate("تاریخ پایان"));

            RuleFor(x => x.ArrivalDate)
                .Must(ValidatorService.IsValidPersianDate)
                .WithMessage(ValidationMessage.IsValidPersianDate("تاریخ ورود"));

            RuleFor(x => x.DepartureDate)
                .Must(ValidatorService.IsValidPersianDate)
                .WithMessage(ValidationMessage.IsValidPersianDate("تاریخ خروج"));

            RuleFor(x => x.UserName)
                .Must(ValidatorService.SanitizeAndValidateInput).WithMessage(ValidationMessage.SanitizeInput("نام کاربری"))
                .When(x => !string.IsNullOrWhiteSpace(x.UserName))
                .MinimumLength(3).WithMessage(ValidationMessage.MinLength("نام کاربری", 3))
                .MaximumLength(200).WithMessage(ValidationMessage.MaxLength("نام کاربری", 200));

            RuleFor(x => x.Ip)
                .Must(ValidatorService.IsValidIp).WithMessage(ValidationMessage.IsValidIpRangeOrCidr())
                .When(x => !string.IsNullOrWhiteSpace(x.Ip));
        }
    }
}
