using FormerUrban_Afta.DataAccess.DTOs.Marahel;
using FormerUrban_Afta.DataAccess.Services;

namespace FormerUrban_Afta.DataAccess.DTOs.Validation.Marahel;

public class ControlMapValidator : AbstractValidator<ControlMapDto>
{
    public ControlMapValidator()
    {
        RuleFor(x => x.shop)
            .NotEmpty().WithMessage(ValidationMessage.Required("شماره پرونده"))
            .GreaterThan(0).WithMessage(ValidationMessage.Required("شماره پرونده"));

        RuleFor(x => x.sh_Darkhast)
            .NotEmpty().WithMessage(ValidationMessage.Required("شماره درخواست"))
            .GreaterThan(0).WithMessage(ValidationMessage.Required("شماره درخواست"));

        RuleFor(x => x.C_NoeNama)
            .NotEmpty().WithMessage(ValidationMessage.Required("نوع نما"))
            .GreaterThan(0).WithMessage(ValidationMessage.MoreThan("نوع نما", 0));

        RuleFor(x => x.NoeNama)
            .MaximumLength(200).When(x => !string.IsNullOrWhiteSpace(x.NoeNama)).WithMessage(ValidationMessage.MaxLength("نوع نما", 200))
            .Must(ValidatorService.SanitizeAndValidateInput).When(x => !string.IsNullOrWhiteSpace(x.NoeNama)).WithMessage(ValidationMessage.SanitizeInput("نوع نما"))
            .Must(ValidatorService.IsAlphanumeric).When(x => !string.IsNullOrWhiteSpace(x.NoeNama)).WithMessage(ValidationMessage.IsAlphanumeric("نوع نما"));

        RuleFor(x => x.c_noesaghf)
            .NotEmpty().WithMessage(ValidationMessage.Required("نوع سقف"))
            .GreaterThanOrEqualTo(0).WithMessage(ValidationMessage.MoreThan("نوع سقف", 0));

        RuleFor(x => x.noesaghf)
            .MaximumLength(200).When(x => !string.IsNullOrWhiteSpace(x.noesaghf)).WithMessage(ValidationMessage.MaxLength("نوع سقف", 200))
            .Must(ValidatorService.SanitizeAndValidateInput).When(x => !string.IsNullOrWhiteSpace(x.noesaghf)).WithMessage(ValidationMessage.SanitizeInput("نوع سقف"))
            .Must(ValidatorService.IsAlphanumeric).When(x => !string.IsNullOrWhiteSpace(x.noesaghf)).WithMessage(ValidationMessage.IsAlphanumeric("نوع سقف"));

        RuleFor(x => x.c_NoeSaze)
            .NotEmpty().WithMessage(ValidationMessage.Required("نوع سازه"))
            .GreaterThanOrEqualTo(0).WithMessage(ValidationMessage.MoreThan("نوع سازه", 0));

        RuleFor(x => x.NoeSaze)
            .MaximumLength(200).When(x => !string.IsNullOrWhiteSpace(x.NoeSaze)).WithMessage(ValidationMessage.MaxLength("نوع سازه", 200))
            .Must(ValidatorService.SanitizeAndValidateInput).When(x => !string.IsNullOrWhiteSpace(x.NoeSaze)).WithMessage(ValidationMessage.SanitizeInput("نوع سازه"))
            .Must(ValidatorService.IsAlphanumeric).When(x => !string.IsNullOrWhiteSpace(x.NoeSaze)).WithMessage(ValidationMessage.IsAlphanumeric("نوع سازه"));

        RuleFor(x => x.masahat_s)
            .Must(ValidatorService.AmountIsValidFormat).WithMessage(ValidationMessage.ValidAmountFormat("مساحت طبق سند"))
            .GreaterThanOrEqualTo(0).WithMessage(ValidationMessage.MoreThan("مساحت طبق سند", 0))
            .LessThanOrEqualTo(1000000000).WithMessage(ValidationMessage.AmountLessThanOrEqualToMax("مساحت طبق سند", "یک میلیارد متر"));
        
        RuleFor(x => x.masahat_e)
                    .Must(ValidatorService.AmountIsValidFormat).WithMessage(ValidationMessage.ValidAmountFormat("مساحت اصلاحی"))
                    .GreaterThanOrEqualTo(0).WithMessage(ValidationMessage.MoreThan("مساحت اصلاحی", 0))
                    .LessThanOrEqualTo(1000000000).WithMessage(ValidationMessage.AmountLessThanOrEqualToMax("مساحت اصلاحی", "یک میلیارد متر"));

        RuleFor(x => x.masahat_m)
            .Must(ValidatorService.AmountIsValidFormat).WithMessage(ValidationMessage.ValidAmountFormat("مساحت موجود"))
            .GreaterThanOrEqualTo(0).WithMessage(ValidationMessage.MoreThan("مساحت موجود", 0))
            .LessThanOrEqualTo(1000000000).WithMessage(ValidationMessage.AmountLessThanOrEqualToMax("مساحت موجود", "یک میلیارد متر"));

        RuleFor(x => x.masahat_b)
            .Must(ValidatorService.AmountIsValidFormat).WithMessage(ValidationMessage.ValidAmountFormat("مساحت باقیمانده"))
            .GreaterThanOrEqualTo(0).WithMessage(ValidationMessage.MoreThan("مساحت باقیمانده", 0))
            .LessThanOrEqualTo(1000000000).WithMessage(ValidationMessage.AmountLessThanOrEqualToMax("مساحت باقیمانده", "یک میلیارد متر"));

        RuleFor(x => x.satheshghal)
            .Must(ValidatorService.AmountIsValidFormat).WithMessage(ValidationMessage.ValidAmountFormat("سطح اشغال"))
            .When(x => x.satheshghal > 0)
            .GreaterThanOrEqualTo(0).WithMessage(ValidationMessage.MoreThan("سطح اشغال", 0))
            .LessThanOrEqualTo(1000000000).WithMessage(ValidationMessage.AmountLessThanOrEqualToMax("سطح اشغال", "یک میلیارد متر"));

        RuleFor(x => x.tarakom)
            .Must(ValidatorService.AmountIsValidFormat).WithMessage(ValidationMessage.ValidAmountFormat("تراکم"))
            .When(x => x.tarakom > 0)
            .GreaterThanOrEqualTo(0).WithMessage(ValidationMessage.MoreThan("تراکم", 0))
            .LessThanOrEqualTo(1000000000).WithMessage(ValidationMessage.AmountLessThanOrEqualToMax("تراکم", "یک میلیارد متر"));

        RuleFor(x => x.TedadTabaghe)
            .InclusiveBetween(0, 99).WithMessage(ValidationMessage.Between("تعداد طبقه", "0", "99"));
    }
}
