using FormerUrban_Afta.DataAccess.DTOs.Marahel;
using FormerUrban_Afta.DataAccess.Services;

namespace FormerUrban_Afta.DataAccess.DTOs.Validation.Marahel;
public class ExpertValidator : AbstractValidator<ExpertDto>
{
    public ExpertValidator()
    {
        RuleFor(x => x.RequestNumber)
            .GreaterThan(0).WithMessage(ValidationMessage.Required("شماره درخواست"));

        RuleFor(x => x.Name)
            .MaximumLength(200).WithMessage(ValidationMessage.MaxLength("نام", 200))
            .Must(ValidatorService.SanitizeAndValidateInput).WithMessage(ValidationMessage.SanitizeInput("نام"))
            .Must(ValidatorService.IsPersianLetters).WithMessage(ValidationMessage.IsPersianLetters("نام"))
            .When(x => !string.IsNullOrWhiteSpace(x.Name))
            .NotEmpty().WithMessage(ValidationMessage.Required("نام"));

        RuleFor(x => x.Family)
            .MaximumLength(200).WithMessage(ValidationMessage.MaxLength("نام خانوادگی", 200))
            .Must(ValidatorService.SanitizeAndValidateInput).WithMessage(ValidationMessage.SanitizeInput("نام خانوادگی"))
            .Must(ValidatorService.IsPersianLetters).WithMessage(ValidationMessage.IsPersianLetters("نام خانوادگی"))
            .When(x => !string.IsNullOrWhiteSpace(x.Family))
            .NotEmpty().WithMessage(ValidationMessage.Required("نام خانوادگی"));

        RuleFor(x => x.DateVisit)
            .Must(ValidatorService.IsValidPersianDate).WithMessage(ValidationMessage.IsValidPersianDate("تاریخ بازدید"))
            .When(x => !string.IsNullOrWhiteSpace(x.DateVisit))
            .NotEmpty().WithMessage(ValidationMessage.Required("تاریخ بازدید"));

    }
}