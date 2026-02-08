using FluentValidation;
using FormerUrban_Afta.DataAccess.Services;

public class MelkValidator : AbstractValidator<MelkDto>
{
    public MelkValidator()
    {
        RuleFor(x => x.shop)
            .GreaterThan(0).WithMessage(ValidationMessage.Required("شماره پرونده ملک"));

        RuleFor(x => x.radif)
            .GreaterThan(0).WithMessage(ValidationMessage.Required("ردیف ملک"));

        RuleFor(x => x.Active)
            .NotNull().WithMessage(ValidationMessage.Required("وضعیت پرونده"));


        RuleFor(x => x.c_noesanad)
            .GreaterThanOrEqualTo(0).WithMessage(ValidationMessage.MoreThan("کد نوع سند", 0));

        RuleFor(x => x.noesanad)
            .MaximumLength(200).When(x => !string.IsNullOrWhiteSpace(x.noesanad)).WithMessage(ValidationMessage.MaxLength("نوع سند", 200))
            .Must(ValidatorService.SanitizeAndValidateInput).When(x => !string.IsNullOrWhiteSpace(x.noesanad)).WithMessage(ValidationMessage.SanitizeInput("نوع سند"))
            .Must(ValidatorService.IsAlphanumeric).When(x => !string.IsNullOrWhiteSpace(x.noesanad)).WithMessage(ValidationMessage.IsAlphanumeric("نوع سند"));


        RuleFor(x => x.c_vazsanad)
            .GreaterThanOrEqualTo(0).WithMessage(ValidationMessage.MoreThan("کد وضعیت سند", 0));

        RuleFor(x => x.vazsanad)
            .MaximumLength(200).When(x => !string.IsNullOrWhiteSpace(x.vazsanad)).WithMessage(ValidationMessage.MaxLength("وضعیت سند", 200))
            .Must(ValidatorService.SanitizeAndValidateInput).When(x => !string.IsNullOrWhiteSpace(x.vazsanad)).WithMessage(ValidationMessage.SanitizeInput("وضعیت سند"))
            .Must(ValidatorService.IsAlphanumeric).When(x => !string.IsNullOrWhiteSpace(x.vazsanad)).WithMessage(ValidationMessage.IsAlphanumeric("وضعیت سند"));


        RuleFor(x => x.c_vazmelk)
            .GreaterThanOrEqualTo(0).WithMessage(ValidationMessage.MoreThan("کد وضعیت ملک", 0));

        RuleFor(x => x.vazmelk)
            .MaximumLength(200).When(x => !string.IsNullOrWhiteSpace(x.vazmelk)).WithMessage(ValidationMessage.MaxLength("وضعیت ملک", 200))
            .Must(ValidatorService.SanitizeAndValidateInput).When(x => !string.IsNullOrWhiteSpace(x.vazmelk)).WithMessage(ValidationMessage.SanitizeInput("وضعیت ملک"))
            .Must(ValidatorService.IsAlphanumeric).When(x => !string.IsNullOrWhiteSpace(x.vazmelk)).WithMessage(ValidationMessage.IsAlphanumeric("وضعیت ملک"));


        RuleFor(x => x.c_noemelk)
            .GreaterThanOrEqualTo(0).WithMessage(ValidationMessage.MoreThan("کد نوع ملک", 0));

        RuleFor(x => x.noemelk)
            .MaximumLength(200).When(x => !string.IsNullOrWhiteSpace(x.noemelk)).WithMessage(ValidationMessage.MaxLength("نوع ملک", 200))
            .Must(ValidatorService.SanitizeAndValidateInput).When(x => !string.IsNullOrWhiteSpace(x.noemelk)).WithMessage(ValidationMessage.SanitizeInput("نوع ملک"))
            .Must(ValidatorService.IsAlphanumeric).When(x => !string.IsNullOrWhiteSpace(x.noemelk)).WithMessage(ValidationMessage.IsAlphanumeric("نوع ملک"));


        RuleFor(x => x.c_marhaleh)
            .GreaterThanOrEqualTo(0).WithMessage(ValidationMessage.MoreThan("کد مرحله ساخت", 0));

        RuleFor(x => x.marhaleh)
            .MaximumLength(200).When(x => !string.IsNullOrWhiteSpace(x.marhaleh)).WithMessage(ValidationMessage.MaxLength("مرحله ساخت", 200))
            .Must(ValidatorService.SanitizeAndValidateInput).When(x => !string.IsNullOrWhiteSpace(x.marhaleh)).WithMessage(ValidationMessage.SanitizeInput("مرحله ساخت"))
            .Must(ValidatorService.IsAlphanumeric).When(x => !string.IsNullOrWhiteSpace(x.marhaleh)).WithMessage(ValidationMessage.IsAlphanumeric("مرحله ساخت"));


        RuleFor(x => x.c_mahdodeh)
            .GreaterThanOrEqualTo(0).WithMessage(ValidationMessage.MoreThan("کد محدوده", 0));

        RuleFor(x => x.mahdodeh)
            .MaximumLength(200).When(x => !string.IsNullOrWhiteSpace(x.mahdodeh)).WithMessage(ValidationMessage.MaxLength("محدوده", 200))
            .Must(ValidatorService.SanitizeAndValidateInput).When(x => !string.IsNullOrWhiteSpace(x.mahdodeh)).WithMessage(ValidationMessage.SanitizeInput("محدوده"))
            .Must(ValidatorService.IsAlphanumeric).When(x => !string.IsNullOrWhiteSpace(x.mahdodeh)).WithMessage(ValidationMessage.IsAlphanumeric("محدوده"));


        RuleFor(x => x.C_karbariAsli)
            .GreaterThanOrEqualTo(0).WithMessage(ValidationMessage.MoreThan("کد کاربری اصلی", 0));

        RuleFor(x => x.KarbariAsli)
            .MaximumLength(200).When(x => !string.IsNullOrWhiteSpace(x.KarbariAsli)).WithMessage(ValidationMessage.MaxLength("کاربری اصلی", 200))
            .Must(ValidatorService.SanitizeAndValidateInput).When(x => !string.IsNullOrWhiteSpace(x.KarbariAsli)).WithMessage(ValidationMessage.SanitizeInput("کاربری اصلی"))
            .Must(ValidatorService.IsAlphanumeric).When(x => !string.IsNullOrWhiteSpace(x.KarbariAsli)).WithMessage(ValidationMessage.IsAlphanumeric("کاربری"));


        RuleFor(x => x.pelakabi)
            .GreaterThanOrEqualTo(0).WithMessage(ValidationMessage.MoreThan("پلاک آبی", 0))
            .LessThanOrEqualTo(999999).WithMessage(ValidationMessage.MaxLength("پلاک آبی", 999999));

        RuleFor(x => x.tel)
            .Must(ValidatorService.IsValidTel).When(x => !string.IsNullOrWhiteSpace(x.tel)).WithMessage(ValidationMessage.IsValidTel());

        RuleFor(x => x.codeposti)
            .Must(ValidatorService.IsValidPostalCode).When(x => !string.IsNullOrWhiteSpace(x.codeposti)).WithMessage(ValidationMessage.IsValidPostalCode());

        RuleFor(x => x.ArzeshArse)
            .Must(ValidatorService.AmountIsValidFormat).When(x => x.ArzeshArse != 0).WithMessage(ValidationMessage.ValidAmountFormat("ارزش عرصه"))
            .GreaterThanOrEqualTo(0).WithMessage(ValidationMessage.MoreThan("ارزش عرصه", 0))
            .LessThanOrEqualTo(200000000000).WithMessage(ValidationMessage.AmountLessThanOrEqualToMax("ارزش عرصه", "دویست میلیارد ریال"));


        RuleFor(x => x.masahat_s)
            .Must(ValidatorService.AmountIsValidFormat).WithMessage(ValidationMessage.ValidAmountFormat("مساحت طبق سند"))
            .GreaterThanOrEqualTo(0).WithMessage(ValidationMessage.MoreThan("مساحت طبق سند", 0))
            .LessThanOrEqualTo(1000000000).WithMessage(ValidationMessage.AmountLessThanOrEqualToMax("مساحت طبق سند", "یک میلیارد متر"));

        RuleFor(x => x.masahat_m)
            .Must(ValidatorService.AmountIsValidFormat).WithMessage(ValidationMessage.ValidAmountFormat("مساحت موجود"))
            .GreaterThanOrEqualTo(0).WithMessage(ValidationMessage.MoreThan("مساحت موجود", 0))
            .LessThanOrEqualTo(1000000000).WithMessage(ValidationMessage.AmountLessThanOrEqualToMax("مساحت موجود", "یک میلیارد متر"));

        RuleFor(x => x.masahat_b)
            .Must(ValidatorService.AmountIsValidFormat).WithMessage(ValidationMessage.ValidAmountFormat("مساحت باقیمانده"))
            .GreaterThanOrEqualTo(0).WithMessage(ValidationMessage.MoreThan("مساحت باقیمانده", 0))
            .LessThanOrEqualTo(1000000000).WithMessage(ValidationMessage.AmountLessThanOrEqualToMax("مساحت باقیمانده", "یک میلیارد متر"));

        RuleFor(x => x.masahat_e)
            .Must(ValidatorService.AmountIsValidFormat).WithMessage(ValidationMessage.ValidAmountFormat("مساحت اصلاحی"))
            .GreaterThanOrEqualTo(0).WithMessage(ValidationMessage.MoreThan("مساحت اصلاحی", 0))
            .LessThanOrEqualTo(1000000000).WithMessage(ValidationMessage.AmountLessThanOrEqualToMax("مساحت اصلاحی", "یک میلیارد متر"));


        RuleFor(x => x.utmx)
            .Must(ValidatorService.IsValidUtmX).When(x => x.utmx != 0).WithMessage(ValidationMessage.ValidUtmX());

        RuleFor(x => x.utmy)
            .Must(ValidatorService.IsValidUtmY).When(x => x.utmy != 0).WithMessage(ValidationMessage.ValidUtmY());


        RuleFor(x => x.tafkiki)
            .InclusiveBetween(0, 999).WithMessage(ValidationMessage.Between("قطعه تفکیکی","0","999"));

        RuleFor(x => x.fari)
            .InclusiveBetween(0, 99999).WithMessage(ValidationMessage.Between("فرعی", "0", "99999"));

        RuleFor(x => x.azFari)
            .InclusiveBetween(0, 99999).WithMessage(ValidationMessage.Between("از فرعی", "0", "99999"));

        RuleFor(x => x.asli)
            .InclusiveBetween(0, 99999).WithMessage(ValidationMessage.Between("اصلی", "0", "99999"));

        RuleFor(x => x.bakhsh)
            .InclusiveBetween(0, 99).WithMessage(ValidationMessage.Between("بخش", "0", "99"));

        RuleFor(x => x.address)
            .MaximumLength(500).When(x => !string.IsNullOrWhiteSpace(x.address)).WithMessage(ValidationMessage.MaxLength("آدرس", 500))
            .Must(ValidatorService.SanitizeAndValidateInput).When(x => !string.IsNullOrWhiteSpace(x.address)).WithMessage(ValidationMessage.SanitizeInput("آدرس"))
            .Must(ValidatorService.IsAlphanumeric).When(x => !string.IsNullOrWhiteSpace(x.address)).WithMessage(ValidationMessage.IsAlphanumeric("آدرس"));

    }
}