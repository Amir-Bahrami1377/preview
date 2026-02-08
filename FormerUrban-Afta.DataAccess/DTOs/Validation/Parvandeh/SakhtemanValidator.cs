using FormerUrban_Afta.DataAccess.Services;

public class SakhtemanValidator : AbstractValidator<SakhtemanDto>
{
    public SakhtemanValidator()
    {
        RuleFor(x => x.shop)
            .GreaterThan(0).WithMessage(ValidationMessage.Required("شماره پرونده ساختمان"));

        RuleFor(x => x.radif)
            .GreaterThan(0).WithMessage(ValidationMessage.Required("ردیف ساختمان"));

        RuleFor(x => x.Active)
            .NotEmpty().WithMessage(ValidationMessage.Required("وضعیت ساختمان"));

        RuleFor(x => x.c_noenama)
            .NotEmpty().WithMessage(ValidationMessage.Required("نوع نما"))
            .GreaterThan(0).WithMessage(ValidationMessage.MoreThan("نوع نما", 0));

        RuleFor(x => x.noenama)
            .MaximumLength(200).When(x => !string.IsNullOrWhiteSpace(x.noenama)).WithMessage(ValidationMessage.MaxLength("نوع نما", 200))
            .Must(ValidatorService.SanitizeAndValidateInput).When(x => !string.IsNullOrWhiteSpace(x.noenama)).WithMessage(ValidationMessage.SanitizeInput("نوع نما"))
            .Must(ValidatorService.IsAlphanumeric).When(x => !string.IsNullOrWhiteSpace(x.noenama)).WithMessage(ValidationMessage.IsAlphanumeric("نوع نما"));

        RuleFor(x => x.c_noesaghf)
            .NotEmpty().WithMessage(ValidationMessage.Required("نوع سقف"))
            .GreaterThanOrEqualTo(0).WithMessage(ValidationMessage.MoreThan("نوع سقف", 0));

        RuleFor(x => x.noesaghf)
            .MaximumLength(200).When(x => !string.IsNullOrWhiteSpace(x.noesaghf)).WithMessage(ValidationMessage.MaxLength("نوع سقف", 200))
            .Must(ValidatorService.SanitizeAndValidateInput).When(x => !string.IsNullOrWhiteSpace(x.noesaghf)).WithMessage(ValidationMessage.SanitizeInput("نوع سقف"))
            .Must(ValidatorService.IsAlphanumeric).When(x => !string.IsNullOrWhiteSpace(x.noesaghf)).WithMessage(ValidationMessage.IsAlphanumeric("نوع سقف"));

        RuleFor(x => x.c_marhaleh)
            .NotEmpty().WithMessage(ValidationMessage.Required("مرحله ساختمانی"))
            .GreaterThanOrEqualTo(0).WithMessage(ValidationMessage.MoreThan("مرحله ساختمانی", 0));

        RuleFor(x => x.marhaleh)
            .MaximumLength(200).When(x => !string.IsNullOrWhiteSpace(x.marhaleh)).WithMessage(ValidationMessage.MaxLength("مرحله ساختمانی", 200))
            .Must(ValidatorService.SanitizeAndValidateInput).When(x => !string.IsNullOrWhiteSpace(x.marhaleh)).WithMessage(ValidationMessage.SanitizeInput("مرحله ساختمانی"))
            .Must(ValidatorService.IsAlphanumeric).When(x => !string.IsNullOrWhiteSpace(x.marhaleh)).WithMessage(ValidationMessage.IsAlphanumeric("مرحله ساختمانی"));

        RuleFor(x => x.c_NoeSakhteman)
            .NotEmpty().WithMessage(ValidationMessage.Required("نوع ساختمان"))
            .GreaterThanOrEqualTo(0).WithMessage(ValidationMessage.MoreThan("نوع ساختمان", 0));

        RuleFor(x => x.NoeSakhteman)
            .MaximumLength(200).When(x => !string.IsNullOrWhiteSpace(x.NoeSakhteman)).WithMessage(ValidationMessage.MaxLength("نوع ساختمان", 200))
            .Must(ValidatorService.SanitizeAndValidateInput).When(x => !string.IsNullOrWhiteSpace(x.NoeSakhteman)).WithMessage(ValidationMessage.SanitizeInput("نوع ساختمان"))
            .Must(ValidatorService.IsAlphanumeric).When(x => !string.IsNullOrWhiteSpace(x.NoeSakhteman)).WithMessage(ValidationMessage.IsAlphanumeric("نوع ساختمان"));

        RuleFor(x => x.c_NoeSaze)
            .NotEmpty().WithMessage(ValidationMessage.Required("نوع سازه"))
            .GreaterThanOrEqualTo(0).WithMessage(ValidationMessage.MoreThan("نوع سازه", 0));

        RuleFor(x => x.NoeSaze)
            .MaximumLength(200).When(x => !string.IsNullOrWhiteSpace(x.NoeSaze)).WithMessage(ValidationMessage.MaxLength("نوع سازه", 200))
            .Must(ValidatorService.SanitizeAndValidateInput).When(x => !string.IsNullOrWhiteSpace(x.NoeSaze)).WithMessage(ValidationMessage.SanitizeInput("نوع سازه"))
            .Must(ValidatorService.IsAlphanumeric).When(x => !string.IsNullOrWhiteSpace(x.NoeSaze)).WithMessage(ValidationMessage.IsAlphanumeric("نوع سازه"));

        RuleFor(x => x.TarikhEhdas)
            .Must(ValidatorService.IsValidPersianDate)
            .WithMessage(ValidationMessage.IsValidPersianDate("تاریخ احداث"));

        RuleFor(x => x.masahatkol)
            .Must(ValidatorService.AmountIsValidFormat).WithMessage(ValidationMessage.ValidAmountFormat("مساحت کل زیربنا"))
            .When(x => x.masahatkol > 0)
            .GreaterThanOrEqualTo(0).WithMessage(ValidationMessage.MoreThan("مساحت کل زیربنا", 0))
            .LessThanOrEqualTo(1000000000).WithMessage(ValidationMessage.AmountLessThanOrEqualToMax("مساحت کل زیربنا", "یک میلیارد متر"));

        RuleFor(x => x.MasahatZirbana)
            .Must(ValidatorService.AmountIsValidFormat).WithMessage(ValidationMessage.ValidAmountFormat("مساحت زیربنا"))
            .When(x => x.MasahatZirbana > 0)
            .GreaterThanOrEqualTo(0).WithMessage(ValidationMessage.MoreThan("مساحت زیربنا", 0))
            .LessThanOrEqualTo(1000000000).WithMessage(ValidationMessage.AmountLessThanOrEqualToMax("مساحت زیربنا", "یک میلیارد متر"));

        RuleFor(x => x.MasahatArse)
            .Must(ValidatorService.AmountIsValidFormat).WithMessage(ValidationMessage.ValidAmountFormat("مساحت سهم العرصه"))
            .When(x => x.MasahatArse > 0)
            .GreaterThanOrEqualTo(0).WithMessage(ValidationMessage.MoreThan("مساحت سهم العرصه", 0))
            .LessThanOrEqualTo(1000000000).WithMessage(ValidationMessage.AmountLessThanOrEqualToMax("مساحت سهم العرصه", "یک میلیارد متر"));

        RuleFor(x => x.tarakom)
            .Must(ValidatorService.AmountIsValidFormat).WithMessage(ValidationMessage.ValidAmountFormat("مساحت مجاز در پروانه"))
            .When(x => x.tarakom > 0)
            .GreaterThanOrEqualTo(0).WithMessage(ValidationMessage.MoreThan("مساحت مجاز در پروانه", 0))
            .LessThanOrEqualTo(1000000000).WithMessage(ValidationMessage.AmountLessThanOrEqualToMax("مساحت مجاز در پروانه", "یک میلیارد متر"));

        RuleFor(x => x.satheshghal)
            .Must(ValidatorService.AmountIsValidFormat).WithMessage(ValidationMessage.ValidAmountFormat("سطح اشغال"))
            .When(x => x.satheshghal > 0)
            .GreaterThanOrEqualTo(0).WithMessage(ValidationMessage.MoreThan("سطح اشغال", 0))
            .LessThanOrEqualTo(1000000000).WithMessage(ValidationMessage.AmountLessThanOrEqualToMax("سطح اشغال", "یک میلیارد متر"));

        RuleFor(x => x.ArzeshAyan)
            .Must(ValidatorService.AmountIsValidFormat).When(x => x.ArzeshAyan != null).WithMessage(ValidationMessage.ValidAmountFormat("ارزش اعیان"))
            .GreaterThanOrEqualTo(0).WithMessage(ValidationMessage.MoreThan("ارزش اعیان", 0))
            .LessThanOrEqualTo(200000000000).WithMessage(ValidationMessage.AmountLessThanOrEqualToMax("ارزش اعیان", "دویست میلیارد ریال"));

        RuleFor(x => x.TedadTabaghe)
            .InclusiveBetween(0,99).WithMessage(ValidationMessage.Between("تعداد طبقه", "0", "99"));
    }
}
