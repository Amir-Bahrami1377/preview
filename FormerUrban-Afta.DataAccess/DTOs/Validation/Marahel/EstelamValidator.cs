using FormerUrban_Afta.DataAccess.DTOs.Marahel;
using FormerUrban_Afta.DataAccess.Services;

namespace FormerUrban_Afta.DataAccess.DTOs.Validation.Marahel;
public class EstelamValidator : AbstractValidator<EstelamDto>
{
    public EstelamValidator()
    {
        RuleFor(x => x.shop)
            .NotEmpty().WithMessage(ValidationMessage.Required("شماره پرونده"))
            .GreaterThan(0).WithMessage(ValidationMessage.Required("شماره پرونده"));

        RuleFor(x => x.Sh_Darkhast)
            .NotEmpty().WithMessage(ValidationMessage.Required("شماره درخواست"))
            .GreaterThan(0).WithMessage(ValidationMessage.Required("شماره درخواست"));


        RuleFor(x => x.codeNoeMalekiat)
            .NotEmpty().WithMessage(ValidationMessage.Required("نوع مالکیت"));

        RuleFor(x => x.NoeMalekiat)
            .MaximumLength(200).WithMessage(ValidationMessage.MaxLength("نوع مالکیت", 200))
            .Must(ValidatorService.SanitizeAndValidateInput).WithMessage(ValidationMessage.SanitizeInput("نوع مالکیت"))
            .Must(ValidatorService.IsAlphanumeric).WithMessage(ValidationMessage.IsAlphanumeric("نوع مالکیت"))
            .When(x => !string.IsNullOrWhiteSpace(x.NoeMalekiat));

        RuleFor(x => x.Tarikh_Pasokh)
            .Must(ValidatorService.IsValidPersianDate).WithMessage(ValidationMessage.IsValidPersianDate("تاریخ پاسخ استعلام"))
            .When(x => !string.IsNullOrWhiteSpace(x.Tarikh_Pasokh))
            .NotEmpty().WithMessage(ValidationMessage.Required("تاریخ پاسخ استعلام"));

        RuleFor(x => x.Sh_Pasokh)
            .MaximumLength(200).WithMessage(ValidationMessage.MaxLength("شماره پاسخ استعلام", 200))
            .Must(ValidatorService.SanitizeAndValidateInput).WithMessage(ValidationMessage.SanitizeInput("شماره پاسخ استعلام"))
            .Must(ValidatorService.IsAlphanumeric).WithMessage(ValidationMessage.IsAlphanumeric("شماره پاسخ استعلام"))
            .When(x => !string.IsNullOrWhiteSpace(x.Sh_Pasokh))
            .NotEmpty().WithMessage(ValidationMessage.Required("شماره پاسخ استعلام"));

        RuleFor(x => x.Dang_Enteghal)
            .InclusiveBetween(0, 6).WithMessage(ValidationMessage.Between("دانگ مورد انتقال", "0", "6"))
            .When(x => x.Dang_Enteghal > 0);

        RuleFor(x => x.Kharidar)
            .Must(ValidatorService.SanitizeAndValidateInput).WithMessage(ValidationMessage.SanitizeInput("نام خریدار"))
            .Must(ValidatorService.IsPersianLetters).WithMessage(ValidationMessage.IsPersianLetters("نام خریدار"))
            .When(x => !string.IsNullOrWhiteSpace(x.Kharidar))
            .NotEmpty().WithMessage(ValidationMessage.Required("نام خریدار"))
            .MaximumLength(200).WithMessage(ValidationMessage.MaxLength("نام خریدار", 200));

        RuleFor(x => x.Tozihat)
            .Must(ValidatorService.SanitizeAndValidateInput).WithMessage(ValidationMessage.SanitizeInput("توضیحات"))
            .When(x => !string.IsNullOrWhiteSpace(x.Tozihat))
            .NotEmpty().WithMessage(ValidationMessage.Required("توضیحات"))
            .MaximumLength(500).WithMessage(ValidationMessage.MaxLength("توضیحات", 500));

    }
}