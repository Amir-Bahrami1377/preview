using FormerUrban_Afta.DataAccess.Services;

public class ApartmanValidator : AbstractValidator<ApartmanDto>
{
    public ApartmanValidator()
    {
        RuleFor(x => x.shop)
            .GreaterThan(0).WithMessage(ValidationMessage.Required("شماره پرونده آپارتمان"));

        RuleFor(x => x.radif)
            .GreaterThan(0).WithMessage(ValidationMessage.Required("ردیف آپارتمان"));

        RuleFor(x => x.Active)
            .NotEmpty().WithMessage(ValidationMessage.Required("وضعیت آپارتمان"));

        RuleFor(x => x.c_noesanad)
    .NotEmpty().WithMessage(ValidationMessage.Required("نوع سند"))
    .GreaterThan(0).WithMessage(ValidationMessage.MoreThan("نوع سند", 0));

        RuleFor(x => x.noesanad)
            .MaximumLength(200).When(x => !string.IsNullOrWhiteSpace(x.noesanad)).WithMessage(ValidationMessage.MaxLength("نوع سند", 200))
            .Must(ValidatorService.SanitizeAndValidateInput).When(x => !string.IsNullOrWhiteSpace(x.noesanad)).WithMessage(ValidationMessage.SanitizeInput("نوع سند"))
            .Must(ValidatorService.IsAlphanumeric).When(x => !string.IsNullOrWhiteSpace(x.noesanad)).WithMessage(ValidationMessage.IsAlphanumeric("نوع سند"));

        RuleFor(x => x.c_vazsanad)
            .NotEmpty().WithMessage(ValidationMessage.Required("وضعیت سند"))
            .GreaterThanOrEqualTo(0).WithMessage(ValidationMessage.MoreThan("وضعیت سند", 0));

        RuleFor(x => x.vazsanad)
            .MaximumLength(200).When(x => !string.IsNullOrWhiteSpace(x.vazsanad)).WithMessage(ValidationMessage.MaxLength("وضعیت سند", 200))
            .Must(ValidatorService.SanitizeAndValidateInput).When(x => !string.IsNullOrWhiteSpace(x.vazsanad)).WithMessage(ValidationMessage.SanitizeInput("وضعیت سند"))
            .Must(ValidatorService.IsAlphanumeric).When(x => !string.IsNullOrWhiteSpace(x.vazsanad)).WithMessage(ValidationMessage.IsAlphanumeric("وضعیت سند"));

        RuleFor(x => x.c_noemalekiyat)
            .NotEmpty().WithMessage(ValidationMessage.Required("نوع مالکیت"))
            .GreaterThanOrEqualTo(0).WithMessage(ValidationMessage.MoreThan("نوع مالکیت", 0));

        RuleFor(x => x.noemalekiyat)
            .MaximumLength(200).When(x => !string.IsNullOrWhiteSpace(x.noemalekiyat)).WithMessage(ValidationMessage.MaxLength("نوع مالکیت", 200))
            .Must(ValidatorService.SanitizeAndValidateInput).When(x => !string.IsNullOrWhiteSpace(x.noemalekiyat)).WithMessage(ValidationMessage.SanitizeInput("نوع مالکیت"))
            .Must(ValidatorService.IsAlphanumeric).When(x => !string.IsNullOrWhiteSpace(x.noemalekiyat)).WithMessage(ValidationMessage.IsAlphanumeric("نوع مالکیت"));

        RuleFor(x => x.c_NoeSaze)
            .NotEmpty().WithMessage(ValidationMessage.Required("نوع سازه"))
            .GreaterThanOrEqualTo(0).WithMessage(ValidationMessage.MoreThan("نوع سازه", 0));

        RuleFor(x => x.NoeSaze)
            .MaximumLength(200).When(x => !string.IsNullOrWhiteSpace(x.NoeSaze)).WithMessage(ValidationMessage.MaxLength("نوع سازه", 200))
            .Must(ValidatorService.SanitizeAndValidateInput).When(x => !string.IsNullOrWhiteSpace(x.NoeSaze)).WithMessage(ValidationMessage.SanitizeInput("نوع سازه"))
            .Must(ValidatorService.IsAlphanumeric).When(x => !string.IsNullOrWhiteSpace(x.NoeSaze)).WithMessage(ValidationMessage.IsAlphanumeric("نوع سازه"));

        RuleFor(x => x.C_Jahat)
            .NotEmpty().WithMessage(ValidationMessage.Required("جهت"))
            .GreaterThanOrEqualTo(0).WithMessage(ValidationMessage.MoreThan("جهت", 0));

        RuleFor(x => x.jahat)
            .MaximumLength(200).When(x => !string.IsNullOrWhiteSpace(x.jahat)).WithMessage(ValidationMessage.MaxLength("جهت", 200))
            .Must(ValidatorService.SanitizeAndValidateInput).When(x => !string.IsNullOrWhiteSpace(x.jahat)).WithMessage(ValidationMessage.SanitizeInput("جهت"))
            .Must(ValidatorService.IsAlphanumeric).When(x => !string.IsNullOrWhiteSpace(x.jahat)).WithMessage(ValidationMessage.IsAlphanumeric("جهت"));

        RuleFor(x => x.pelakabi)
            .GreaterThanOrEqualTo(0).WithMessage(ValidationMessage.MoreThan("پلاک آبی", 0))
            .LessThanOrEqualTo(999999).WithMessage(ValidationMessage.MaxLength("پلاک آبی", 999999));

        RuleFor(x => x.tel)
            .Must(ValidatorService.IsValidTel).WithMessage(ValidationMessage.IsValidTel())
            .When(x => !string.IsNullOrWhiteSpace(x.tel))
            .NotEmpty().WithMessage(ValidationMessage.Required("شماره تلفن"));

        RuleFor(x => x.codeposti)
            .Must(ValidatorService.IsValidPostalCode).WithMessage(ValidationMessage.IsValidPostalCode())
            .When(x => !string.IsNullOrWhiteSpace(x.codeposti))
            .NotEmpty().WithMessage(ValidationMessage.Required("کد پستی"));

        RuleFor(x => x.MasahatKol)
            .Must(ValidatorService.AmountIsValidFormat).WithMessage(ValidationMessage.ValidAmountFormat("مساحت کل"))
            .When(x => x.MasahatKol > 0)
            .GreaterThanOrEqualTo(0).WithMessage(ValidationMessage.MoreThan("مساحت کل", 0))
            .LessThanOrEqualTo(1000000000).WithMessage(ValidationMessage.AmountLessThanOrEqualToMax("مساحت کل", "یک میلیارد متر"));

        RuleFor(x => x.MasahatArse)
            .Must(ValidatorService.AmountIsValidFormat).WithMessage(ValidationMessage.ValidAmountFormat("مساحت العرصه"))
            .When(x => x.MasahatArse > 0)
            .GreaterThanOrEqualTo(0).WithMessage(ValidationMessage.MoreThan("مساحت العرصه", 0))
            .LessThanOrEqualTo(1000000000).WithMessage(ValidationMessage.AmountLessThanOrEqualToMax("مساحت العرصه", "یک میلیارد متر"));

        RuleFor(x => x.tafkiki)
            .InclusiveBetween(0, 999).WithMessage(ValidationMessage.Between("قطعه تفکیکی", "0", "999"));

        RuleFor(x => x.fari)
            .InclusiveBetween(0, 99999).WithMessage(ValidationMessage.Between("فرعی", "0", "99999"));

        RuleFor(x => x.azFari)
            .InclusiveBetween(0, 99999).WithMessage(ValidationMessage.Between("از فرعی", "0", "99999"));

        RuleFor(x => x.asli)
            .InclusiveBetween(0, 99999).WithMessage(ValidationMessage.Between("اصلی", "0", "99999"));

        RuleFor(x => x.bakhsh)
            .InclusiveBetween(0, 99).WithMessage(ValidationMessage.Between("بخش", "0", "99"));

        RuleFor(x => x.address)
            .Must(ValidatorService.SanitizeAndValidateInput).WithMessage(ValidationMessage.SanitizeInput("آدرس"))
            .Must(ValidatorService.IsAlphanumeric).WithMessage(ValidationMessage.IsAlphanumeric("آدرس"))
            .When(x => !string.IsNullOrWhiteSpace(x.address))
            .NotEmpty().WithMessage(ValidationMessage.Required("آدرس"))
            .MaximumLength(500).WithMessage(ValidationMessage.MaxLength("آدرس", 500));

    }
}
