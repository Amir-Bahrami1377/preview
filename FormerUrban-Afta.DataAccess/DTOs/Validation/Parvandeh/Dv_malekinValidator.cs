using FormerUrban_Afta.DataAccess.Services;

namespace FormerUrban_Afta.DataAccess.DTOs.Validation.Parvandeh
{
    public class Dv_malekinValidator : AbstractValidator<Dv_malekinDTO>
    {
        public Dv_malekinValidator()
        {
            RuleFor(x => x.Identity)
                .GreaterThanOrEqualTo(0).WithMessage(ValidationMessage.MoreThan("کد مالک", 0));

            RuleFor(x => x.id)
                .GreaterThanOrEqualTo(0).WithMessage(ValidationMessage.MoreThan("شماره مالک", 0));

            RuleFor(x => x.shop)
                .GreaterThan(0).WithMessage(ValidationMessage.MoreThan("شماره پرونده مالک", 0));

            RuleFor(x => x.d_radif)
                .GreaterThan(0).WithMessage(ValidationMessage.MoreThan("ردیف مالک", 0));

            RuleFor(x => x.mtable_name)
                .Must(ValidatorService.SanitizeAndValidateInput).WithMessage(ValidationMessage.SanitizeInput("نام جدول"))
                .Must(ValidatorService.IsAlphanumeric).WithMessage(ValidationMessage.IsAlphanumeric("نام جدول"))
                .When(x => !string.IsNullOrWhiteSpace(x.mtable_name))
                .NotEmpty().WithMessage(ValidationMessage.Required("نام جدول"))
                .MaximumLength(200).WithMessage(ValidationMessage.MaxLength("نام جدول", 200));

            RuleFor(x => x.c_noemalek)
                .InclusiveBetween(1, 2).WithMessage(ValidationMessage.Required("نوع مالک"));

            RuleFor(x => x.name)
                .MaximumLength(200).WithMessage(ValidationMessage.MaxLength("نام", 200))
                .Must(ValidatorService.SanitizeAndValidateInput).WithMessage(ValidationMessage.SanitizeInput("نام"))
                .Must(ValidatorService.IsPersianLetters).WithMessage(ValidationMessage.IsPersianLetters("نام"))
                .When(x => !string.IsNullOrWhiteSpace(x.name))
                .NotEmpty().WithMessage(ValidationMessage.Required("نام"));

            RuleFor(x => x.family)
                .MaximumLength(200).WithMessage(ValidationMessage.MaxLength("نام خانوادگی", 200))
                .Must(ValidatorService.SanitizeAndValidateInput).WithMessage(ValidationMessage.SanitizeInput("نام خانوادگی"))
                .Must(ValidatorService.IsPersianLetters).WithMessage(ValidationMessage.IsPersianLetters("نام خانوادگی"))
                .When(x => !string.IsNullOrWhiteSpace(x.family))
                .NotEmpty().WithMessage(ValidationMessage.Required("نام خانوادگی"));

            RuleFor(x => x.father)
                .MaximumLength(200).WithMessage(ValidationMessage.MaxLength("نوع سازه", 200))
                .Must(ValidatorService.SanitizeAndValidateInput).WithMessage(ValidationMessage.SanitizeInput("نوع سازه"))
                .Must(ValidatorService.IsPersianLetters).WithMessage(ValidationMessage.IsPersianLetters("نوع سازه"))
                .When(x => !string.IsNullOrWhiteSpace(x.father));

            RuleFor(x => x.kodemeli)
                .NotEmpty().WithMessage(ValidationMessage.Required("کد ملی"))
                .Must(ValidatorService.IsValidNationalCode).WithMessage(ValidationMessage.IsValidNationalCode())
                .When(x => x.c_noemalek == 1);

            RuleFor(x => x.kodemeli)
                .NotEmpty().WithMessage(ValidationMessage.Required("شناسه ملی"))
                .Must(ValidatorService.IsValidLegalNationalId).WithMessage(ValidationMessage.IsValidLegalNationalId())
                .When(x => x.c_noemalek == 2);

            RuleFor(x => x.sh_sh)
                .Must(ValidatorService.IsValidBirthCertificateNumber).WithMessage(ValidationMessage.MaxLength("شماره شناسنامه", 10))
                .Must(ValidatorService.SanitizeAndValidateInput).WithMessage(ValidationMessage.SanitizeInput("شماره شناسنامه"))
                .Must(ValidatorService.IsAlphanumeric).WithMessage(ValidationMessage.IsAlphanumeric("شماره شناسنامه"))
                .When(x => !string.IsNullOrWhiteSpace(x.sh_sh));

            RuleFor(x => x.mob)
                .Must(ValidatorService.IsValidMobileNumber).WithMessage(ValidationMessage.IsValidMobileNumber())
                .When(x => !string.IsNullOrWhiteSpace(x.mob))
                .NotEmpty().WithMessage(ValidationMessage.Required("شماره همراه"));

            RuleFor(x => x.tel)
                .Must(ValidatorService.IsValidTel).WithMessage(ValidationMessage.IsValidTel())
                .When(x => !string.IsNullOrWhiteSpace(x.tel));

            RuleFor(x => x.sahm_a)
                .InclusiveBetween(0, 100).WithMessage(ValidationMessage.Between("سهم عرصه", "0", "100"))
                .When(x => x.sahm_a > 0);

            RuleFor(x => x.dong_a)
                .InclusiveBetween(0, 6).WithMessage(ValidationMessage.Between("دانگ عرصه", "0", "6"))
                .When(x => x.dong_a > 0);

            RuleFor(x => x.sahm_b)
                .InclusiveBetween(0, 100).WithMessage(ValidationMessage.Between("سهم اعیان", "0", "100"))
                .When(x => x.sahm_b > 0);

            RuleFor(x => x.dong_b)
                .InclusiveBetween(0, 6).WithMessage(ValidationMessage.Between("دانگ اعیان", "0", "6"))
                .When(x => x.dong_b > 0);

            RuleFor(x => x.ArzeshArse)
                .Must(ValidatorService.AmountIsValidFormat).WithMessage(ValidationMessage.ValidAmountFormat("ارزش عرصه"))
                .GreaterThanOrEqualTo(0).WithMessage(ValidationMessage.MoreThan("ارزش عرصه", 0))
                .LessThanOrEqualTo(200000000000).WithMessage(ValidationMessage.AmountLessThanOrEqualToMax("ارزش عرصه", "دویست میلیارد ریال"))
                .When(x => x.ArzeshArse != null);

            RuleFor(x => x.ArzeshAyan)
                .Must(ValidatorService.AmountIsValidFormat).WithMessage(ValidationMessage.ValidAmountFormat("ارزش اعیان"))
                .GreaterThanOrEqualTo(0).WithMessage(ValidationMessage.MoreThan("ارزش اعیان", 0))
                .LessThanOrEqualTo(200000000000).WithMessage(ValidationMessage.AmountLessThanOrEqualToMax("ارزش اعیان", "دویست میلیارد ریال"))
                .When(x => x.ArzeshAyan != null);

            RuleFor(x => x.address)
                .Must(ValidatorService.SanitizeAndValidateInput).WithMessage(ValidationMessage.SanitizeInput("آدرس"))
                .Must(ValidatorService.IsAlphanumeric).WithMessage(ValidationMessage.IsAlphanumeric("آدرس"))
                .When(x => !string.IsNullOrWhiteSpace(x.address))
                .NotEmpty().WithMessage(ValidationMessage.Required("آدرس"))
                .MaximumLength(500).WithMessage(ValidationMessage.MaxLength("آدرس", 500));
        }
    }
}
