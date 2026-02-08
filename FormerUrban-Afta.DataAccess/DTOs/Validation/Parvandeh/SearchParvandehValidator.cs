using FluentValidation;
using FormerUrban_Afta.DataAccess.Services;

namespace FormerUrban_Afta.DataAccess.DTOs.Validation.Parvandeh;

public class SearchParvandehValidator : AbstractValidator<SearchParvandehDto>
{
    public SearchParvandehValidator()
    {
        RuleSet("shop", () =>
        {
            RuleFor(x => x.Value)
            .NotEmpty().When(x => x.Code > 0).WithMessage(ValidationMessage.Required("شماره پرونده"));

            RuleFor(x => x.Value)
            .MaximumLength(20).When(x => !string.IsNullOrWhiteSpace(x.Value)).WithMessage(ValidationMessage.MaxLength("شماره پرونده", 20))
            .Must(ValidatorService.IsDigitsOnly).When(x => !string.IsNullOrWhiteSpace(x.Value)).WithMessage(ValidationMessage.OnlyDigits("شماره پرونده"));
        });

        RuleSet("codeNosazi", () =>
        {
            RuleFor(x => x.Value)
                .NotEmpty().When(x => x.Code > 0).WithMessage(ValidationMessage.Required("کد نوسازی"));

            RuleFor(x => x.Value)
                .MaximumLength(50).When(x => !string.IsNullOrWhiteSpace(x.Value)).WithMessage(ValidationMessage.MaxLength("کد نوسازی", 50))
                .Must(ValidatorService.IsValidCodeNosazi).When(x => !string.IsNullOrWhiteSpace(x.Value)).WithMessage(ValidationMessage.InvalidCodeNosazi);
        });
    }
}

