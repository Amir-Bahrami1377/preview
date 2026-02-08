using FormerUrban_Afta.DataAccess.Services;

public class DarkhastValidator : AbstractValidator<DarkhastDTO>
{
    public DarkhastValidator()
    {
        RuleFor(x => x.shodarkhast)
            .GreaterThan(0).WithMessage(ValidationMessage.Required("شماره درخواست"));

        RuleFor(x => x.shop)
            .GreaterThan(0).WithMessage(ValidationMessage.Required("شماره پرونده"));

        RuleFor(x => x.c_nosazi)
            .Must(ValidatorService.IsValidCodeNosazi).WithMessage(ValidationMessage.InvalidCodeNosazi);

        RuleFor(x => x.c_noemot)
            .GreaterThan(0).WithMessage(ValidationMessage.Required("نوع متقاضی"));

        RuleFor(x => x.noemot)
            .NotEmpty().When(x => x.c_noemot > 0).WithMessage(ValidationMessage.Required("نوع متقاضی"))
            .MaximumLength(200).When(x => x.c_noemot > 0).WithMessage(ValidationMessage.MaxLength("نوع متقاضی", 200))
            .Must(ValidatorService.SanitizeAndValidateInput).When(x => x.c_noemot > 0).WithMessage(ValidationMessage.SanitizeInput("نوع متقاضی"))
            .Must(ValidatorService.IsAlphanumeric).When(x => x.c_noemot > 0).WithMessage(ValidationMessage.IsAlphanumeric("نوع متقاضی"));

        RuleFor(x => x.c_noedarkhast)
            .GreaterThan(0).WithMessage(ValidationMessage.Required("نوع درخواست"));

        RuleFor(x => x.moteghazi)
            .Must(ValidatorService.SanitizeAndValidateInput).WithMessage(ValidationMessage.SanitizeInput("نام متقاضی"))
            .Must(ValidatorService.IsPersianLetters).WithMessage(ValidationMessage.IsPersianLetters("نام متقاضی"))
            .When(x => !string.IsNullOrWhiteSpace(x.moteghazi))
            .NotEmpty().WithMessage(ValidationMessage.Required("نام متقاضی"))
            .MaximumLength(200).WithMessage(ValidationMessage.MaxLength("نام متقاضی", 200));

        RuleFor(x => x.CodeMeli)
            .NotEmpty().WithMessage(ValidationMessage.Required("کد ملی"))
            .Must(ValidatorService.IsValidNationalCode).WithMessage(ValidationMessage.IsValidNationalCode());

        RuleFor(x => x.tel)
            .Must(ValidatorService.IsValidTel).When(x => !string.IsNullOrWhiteSpace(x.tel)).WithMessage(ValidationMessage.IsValidTel());

        RuleFor(x => x.mob)
            .Must(ValidatorService.IsValidMobileNumber).When(x => !string.IsNullOrWhiteSpace(x.mob)).WithMessage(ValidationMessage.IsValidMobileNumber())
            .NotEmpty().WithMessage(ValidationMessage.Required("شماره همراه"));

        RuleFor(x => x.codeposti)
            .Must(ValidatorService.IsValidPostalCode).When(x => !string.IsNullOrWhiteSpace(x.codeposti)).WithMessage(ValidationMessage.IsValidPostalCode());

        RuleFor(x => x.email)
            .Must(ValidatorService.SanitizeAndValidateInput).WithMessage(ValidationMessage.SanitizeInput("ایمیل"))
            .MaximumLength(320).WithMessage(ValidationMessage.MaxLength("ایمیل", 320))
            .EmailAddress().WithMessage(ValidationMessage.IsValidEmail())
            .When(x => !string.IsNullOrWhiteSpace(x.email));

        RuleFor(x => x.address)
            .Must(ValidatorService.SanitizeAndValidateInput).WithMessage(ValidationMessage.SanitizeInput("آدرس"))
            .When(x => !string.IsNullOrWhiteSpace(x.address))
            .NotEmpty().WithMessage(ValidationMessage.Required("آدرس"))
            .MaximumLength(500).WithMessage(ValidationMessage.MaxLength("آدرس", 500));
    }
}