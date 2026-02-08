using FormerUrban_Afta.DataAccess.Services;

namespace FormerUrban_Afta.DataAccess.DTOs.Validation.Setting;

public class AllowedIPRangeDtoValidator : AbstractValidator<AllowedIPRangeDto>
{
    public AllowedIPRangeDtoValidator()
    {
        RuleFor(x => x.IPRange)
            .Must(ValidatorService.IsValidIpRangeOrCidr).WithMessage(ValidationMessage.IsValidIpRangeOrCidr())
            .When(x => !string.IsNullOrWhiteSpace(x.IPRange))
            .NotEmpty().WithMessage(ValidationMessage.Required("رنج IP"));

        RuleFor(x => x.FromDate)
            .Must(ValidatorService.IsValidPersianDateTime).WithMessage(ValidationMessage.IsValidPersianDate("تاریخ شروع"))
            .When(x => !string.IsNullOrWhiteSpace(x.FromDate))
            .NotEmpty().WithMessage(ValidationMessage.Required("تاریخ شروع"));

        RuleFor(x => x.ToDate)
            .Must(ValidatorService.IsValidPersianDateTime).WithMessage(ValidationMessage.IsValidPersianDate("تاریخ پایان"))
            .When(x => !string.IsNullOrWhiteSpace(x.FromDate))
            .NotEmpty().WithMessage(ValidationMessage.Required("تاریخ پایان"));

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage(ValidationMessage.MaxLength("توضیحات", 500))
            .Must(ValidatorService.SanitizeAndValidateInput).WithMessage(ValidationMessage.SanitizeInput("توضیحات"))
            .Must(ValidatorService.IsAlphanumeric).WithMessage(ValidationMessage.IsAlphanumeric("توضیحات"))
            .When(x => !string.IsNullOrWhiteSpace(x.Description))
            .NotEmpty().WithMessage(ValidationMessage.Required("توضیحات"));


        RuleFor(x => x)
            .Must(x =>
            {
                if (x.FromDate == null || x.ToDate == null)
                    return true;

                var from = ValidatorService.ParsePersianDateTime(x.FromDate!);
                var to = ValidatorService.ParsePersianDateTime(x.ToDate!);
                return from <= to;
            })
            .WithMessage("تاریخ شروع نباید از تاریخ پایان بزرگتر باشد!");
    }
}

