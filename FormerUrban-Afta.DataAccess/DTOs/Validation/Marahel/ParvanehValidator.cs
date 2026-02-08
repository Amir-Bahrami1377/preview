using FluentValidation;
using FormerUrban_Afta.DataAccess.DTOs.Marahel;
using FormerUrban_Afta.DataAccess.Services;

namespace FormerUrban_Afta.DataAccess.DTOs.Validation.Marahel;

public class ParvanehValidator : AbstractValidator<ParvanehDto>
{
    public ParvanehValidator()
    {
        RuleFor(x => x.shop)
            .NotEmpty().WithMessage(ValidationMessage.Required("شماره پرونده"))
            .GreaterThan(0).WithMessage(ValidationMessage.Required("شماره پرونده"));

        RuleFor(x => x.sh_darkhast)
            .NotEmpty().WithMessage(ValidationMessage.Required("شماره درخواست"))
            .GreaterThan(0).WithMessage(ValidationMessage.Required("شماره درخواست"));


        RuleFor(x => x.c_noeParvaneh)
            .NotEmpty().WithMessage(ValidationMessage.Required("نوع پروانه"));

        RuleFor(x => x.noe_parvaneh)
            .MaximumLength(200).WithMessage(ValidationMessage.MaxLength("نوع پروانه", 200))
            .Must(ValidatorService.SanitizeAndValidateInput).WithMessage(ValidationMessage.SanitizeInput("نوع پروانه"))
            .Must(ValidatorService.IsAlphanumeric).WithMessage(ValidationMessage.IsAlphanumeric("نوع پروانه"))
            .When(x => !string.IsNullOrWhiteSpace(x.noe_parvaneh));

        RuleFor(x => x.tarikh_parvaneh)
            .Must(ValidatorService.IsValidPersianDate).WithMessage(ValidationMessage.IsValidPersianDate("تاریخ پاسخ استعلام"))
            .When(x => !string.IsNullOrWhiteSpace(x.tarikh_parvaneh))
            .NotEmpty().WithMessage(ValidationMessage.Required("تاریخ پاسخ استعلام"));

        RuleFor(x => x.tarikh_end_amaliat_s)
            .Must(ValidatorService.IsValidPersianDate).WithMessage(ValidationMessage.IsValidPersianDate("تاریخ اتمام عملیات"))
            .When(x => !string.IsNullOrWhiteSpace(x.tarikh_parvaneh))
            .NotEmpty().WithMessage(ValidationMessage.Required("تاریخ اتمام عملیات"));

        RuleFor(x => x.tarikh_e_bimeh)
            .Must(ValidatorService.IsValidPersianDate).WithMessage(ValidationMessage.IsValidPersianDate("تاریخ اعتبار بیمه"))
            .When(x => !string.IsNullOrWhiteSpace(x.tarikh_parvaneh))
            .NotEmpty().WithMessage(ValidationMessage.Required("تاریخ اعتبار بیمه"));

        RuleFor(x => x.sho_parvaneh)
            .Must(x => ValidatorService.MaxLength(x.ToString(), 8)).WithMessage(ValidationMessage.MaxLength("شماره پروانه", 8));

        RuleFor(x => x.sho_bimenameh)
            .Must(x => ValidatorService.Length(x.ToString(), 8)).WithMessage(ValidationMessage.Length("شماره بیمه کارگران", 8));

        RuleFor(x => x.masahat_m_esh_zamin)
            .Must(ValidatorService.AmountIsValidFormat).WithMessage(ValidationMessage.ValidAmountFormat("مساحت زمین"))
            .GreaterThanOrEqualTo(0).WithMessage(ValidationMessage.MoreThan("مساحت زمین", 0))
            .LessThanOrEqualTo(1000000000).WithMessage(ValidationMessage.AmountLessThanOrEqualToMax("مساحت زمین", "یک میلیارد متر"));
        
        RuleFor(x => x.masahat_m_s_tarakom)
            .Must(ValidatorService.AmountIsValidFormat).WithMessage(ValidationMessage.ValidAmountFormat("مساحت تراکم"))
            .GreaterThanOrEqualTo(0).WithMessage(ValidationMessage.MoreThan("مساحت تراکم", 0))
            .LessThanOrEqualTo(1000000000).WithMessage(ValidationMessage.AmountLessThanOrEqualToMax("مساحت تراکم", "یک میلیارد متر"));

        RuleFor(x => x.tozihat_parvaneh)
            .MaximumLength(500).WithMessage(ValidationMessage.MaxLength("توضیحات", 500))
            .Must(ValidatorService.SanitizeAndValidateInput).WithMessage(ValidationMessage.SanitizeInput("توضیحات"))
            .Must(ValidatorService.IsAlphanumeric).WithMessage(ValidationMessage.IsAlphanumeric("توضیحات"))
            .When(x => !string.IsNullOrWhiteSpace(x.tozihat_parvaneh));
    }
}
