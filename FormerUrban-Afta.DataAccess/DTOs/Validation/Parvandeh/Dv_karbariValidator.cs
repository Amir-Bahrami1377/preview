using FormerUrban_Afta.DataAccess.Services;

public class Dv_karbariValidator : AbstractValidator<Dv_karbariDTO>
{
    public Dv_karbariValidator()
    {
        RuleFor(x => x.Identity)
            .GreaterThanOrEqualTo(0).WithMessage(ValidationMessage.MoreThan("کد کاربری", 0));

        RuleFor(x => x.id)
            .GreaterThanOrEqualTo(0).WithMessage(ValidationMessage.MoreThan("شماره کاربری", 0));

        RuleFor(x => x.shop)
            .GreaterThanOrEqualTo(0).WithMessage(ValidationMessage.MoreThan("شماره پرونده کاربری", 0));

        RuleFor(x => x.d_radif)
            .GreaterThanOrEqualTo(0).WithMessage(ValidationMessage.MoreThan("ردیف کاربری", 0));

        RuleFor(x => x.mtable_name)
            .Must(ValidatorService.SanitizeAndValidateInput).WithMessage(ValidationMessage.SanitizeInput("نام جدول"))
            .Must(ValidatorService.IsAlphanumeric).WithMessage(ValidationMessage.IsAlphanumeric("نام جدول"))
            .When(x => !string.IsNullOrWhiteSpace(x.mtable_name))
            .NotEmpty().WithMessage(ValidationMessage.Required("نام جدول"))
            .MaximumLength(200).WithMessage(ValidationMessage.MaxLength("نام جدول", 200));

        RuleFor(x => x.CodeMarhale)
            .GreaterThanOrEqualTo(0).WithMessage(ValidationMessage.MoreThan("کد مرحله", 0));

        RuleFor(x => x.c_tabagheh)
            .GreaterThanOrEqualTo(0).WithMessage(ValidationMessage.MoreThan("طبقه", 0));

        RuleFor(x => x.tabagheh)
            .MaximumLength(200).WithMessage(ValidationMessage.MaxLength("طبقه", 200))
            .Must(ValidatorService.SanitizeAndValidateInput).WithMessage(ValidationMessage.SanitizeInput("طبقه"))
            .Must(ValidatorService.IsAlphanumeric).WithMessage(ValidationMessage.IsAlphanumeric("طبقه"))
            .When(x => !string.IsNullOrWhiteSpace(x.tabagheh))
            .NotEmpty().WithMessage(ValidationMessage.Required("طبقه"));

        RuleFor(x => x.c_karbari)
            .GreaterThanOrEqualTo(0).WithMessage(ValidationMessage.MoreThan("کاربری", 0));

        RuleFor(x => x.karbari)
            .MaximumLength(200).WithMessage(ValidationMessage.MaxLength("کاربری", 200))
            .Must(ValidatorService.SanitizeAndValidateInput).WithMessage(ValidationMessage.SanitizeInput("کاربری"))
            .Must(ValidatorService.IsAlphanumeric).WithMessage(ValidationMessage.IsAlphanumeric("کاربری"))
            .When(x => !string.IsNullOrWhiteSpace(x.karbari))
            .NotEmpty().WithMessage(ValidationMessage.Required("کاربری"));

        RuleFor(x => x.c_noeestefadeh)
            .GreaterThanOrEqualTo(0).WithMessage(ValidationMessage.MoreThan("نوع استفاده", 0));

        RuleFor(x => x.noeestefadeh)
            .MaximumLength(200).WithMessage(ValidationMessage.MaxLength("نوع استفاده", 200))
            .Must(ValidatorService.SanitizeAndValidateInput).WithMessage(ValidationMessage.SanitizeInput("نوع استفاده"))
            .Must(ValidatorService.IsAlphanumeric).WithMessage(ValidationMessage.IsAlphanumeric("نوع استفاده"))
            .When(x => !string.IsNullOrWhiteSpace(x.noeestefadeh))
            .NotEmpty().WithMessage(ValidationMessage.Required("نوع استفاده"));

        RuleFor(x => x.c_noesakhteman)
            .GreaterThanOrEqualTo(0).WithMessage(ValidationMessage.MoreThan("نوع ساختمان", 0));

        RuleFor(x => x.noesakhteman)
            .MaximumLength(200).WithMessage(ValidationMessage.MaxLength("نوع ساختمان", 200))
            .Must(ValidatorService.SanitizeAndValidateInput).WithMessage(ValidationMessage.SanitizeInput("نوع ساختمان"))
            .Must(ValidatorService.IsAlphanumeric).WithMessage(ValidationMessage.IsAlphanumeric("نوع ساختمان"))
            .When(x => !string.IsNullOrWhiteSpace(x.noesakhteman))
            .NotEmpty().WithMessage(ValidationMessage.Required("نوع ساختمان"));

        RuleFor(x => x.c_noesazeh)
            .GreaterThanOrEqualTo(0).WithMessage(ValidationMessage.MoreThan("نوع سازه", 0));

        RuleFor(x => x.noesazeh)
            .MaximumLength(200).WithMessage(ValidationMessage.MaxLength("نوع سازه", 200))
            .Must(ValidatorService.SanitizeAndValidateInput).WithMessage(ValidationMessage.SanitizeInput("نوع سازه"))
            .Must(ValidatorService.IsAlphanumeric).WithMessage(ValidationMessage.IsAlphanumeric("نوع سازه"))
            .When(x => !string.IsNullOrWhiteSpace(x.noesazeh))
            .NotEmpty().WithMessage(ValidationMessage.Required("نوع سازه"));

        RuleFor(x => x.c_marhaleh)
            .GreaterThanOrEqualTo(0).WithMessage(ValidationMessage.MoreThan("مرحله", 0));

        RuleFor(x => x.marhaleh)
            .MaximumLength(200).WithMessage(ValidationMessage.MaxLength("مرحله", 200))
            .Must(ValidatorService.SanitizeAndValidateInput).WithMessage(ValidationMessage.SanitizeInput("مرحله"))
            .Must(ValidatorService.IsAlphanumeric).WithMessage(ValidationMessage.IsAlphanumeric("مرحله"))
            .When(x => !string.IsNullOrWhiteSpace(x.marhaleh))
            .NotEmpty().WithMessage(ValidationMessage.Required("مرحله"));

        RuleFor(x => x.tarikhehdas)
            .NotEmpty().WithMessage(ValidationMessage.Required("تاریخ احداث"))
            .Must(ValidatorService.IsValidPersianDate).WithMessage(ValidationMessage.IsValidPersianDate("تاریخ احداث"));

        RuleFor(x => x.masahat_k)
            .Must(ValidatorService.AmountIsValidFormat).WithMessage(ValidationMessage.ValidAmountFormat("مساحت کاربری"))
            .When(x => x.masahat_k > 0)
            .NotEmpty().WithMessage(ValidationMessage.Required("مساحت کاربری"))
            .GreaterThan(0).WithMessage(ValidationMessage.MoreThan("مساحت کاربری", 0))
            .LessThanOrEqualTo(1000000000).WithMessage(ValidationMessage.AmountLessThanOrEqualToMax("مساحت کاربری", "یک میلیارد متر"));
    }
}
