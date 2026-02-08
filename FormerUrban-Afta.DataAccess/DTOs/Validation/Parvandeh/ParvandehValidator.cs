using FormerUrban_Afta.DataAccess.Services;

public class ParvandehValidator : AbstractValidator<ParvandehDto>
{
    public ParvandehValidator()
    {
        RuleSet("melk", () =>
        {
            RuleFor(x => x.Index)
                .Must(x => x is >= 2 or <= 4).WithMessage(ValidationMessage.Between("کد نوع پرونده", "2", "4"));

            //RuleFor(x => x.shop)
            //    .Equal(0).WithMessage(ValidationMessage.Equal("شماره پرونده والد", "0"));

            RuleFor(x => x.mantaghe)
                .GreaterThan(0).WithMessage(ValidationMessage.MoreThan("منطقه", 0))
                .Must(x => ValidatorService.MaxLength(x.ToString(), 7)).WithMessage(ValidationMessage.MaxLength("منطقه", 7));

            RuleFor(x => x.hoze)
                .GreaterThan(0).WithMessage(ValidationMessage.MoreThan("محله", 0))
                .Must(x => ValidatorService.MaxLength(x.ToString(), 7)).WithMessage(ValidationMessage.MaxLength("محله", 7));

            RuleFor(x => x.blok)
                .GreaterThan(0).WithMessage(ValidationMessage.MoreThan("بلوک", 0))
                .Must(x => ValidatorService.MaxLength(x.ToString(), 7)).WithMessage(ValidationMessage.MaxLength("بلوک", 7));

            RuleFor(x => x.Melk)
                .GreaterThan(0).WithMessage(ValidationMessage.MoreThan("ملک", 0))
                .Must(x => ValidatorService.MaxLength(x.ToString(), 7)).WithMessage(ValidationMessage.MaxLength("ملک", 7));

            RuleFor(x => x.sakhteman)
                .Equal(0).WithMessage(ValidationMessage.Equal("ساختمان", "0"));

            RuleFor(x => x.apar)
                .Equal(0).WithMessage(ValidationMessage.Equal("آپارتمان", "0"));
        });

        RuleSet("sakhteman", () =>
        {
            RuleFor(x => x.Index)
                .Must(x => x is >= 2 or <= 4).WithMessage(ValidationMessage.Between("کد نوع پرونده", "2", "4"));

            RuleFor(x => x.shop)
                .GreaterThan(0).WithMessage(ValidationMessage.MoreThan("شماره پرونده والد", 0))
                .Must(x => ValidatorService.MaxLength(x.ToString(), 7)).WithMessage(ValidationMessage.MaxLength("شماره پرونده والد", 10));

            RuleFor(x => x.mantaghe)
                .GreaterThan(0).WithMessage(ValidationMessage.MoreThan("منطقه", 0))
                .Must(x => ValidatorService.MaxLength(x.ToString(), 7)).WithMessage(ValidationMessage.MaxLength("منطقه", 7));

            RuleFor(x => x.hoze)
                .GreaterThan(0).WithMessage(ValidationMessage.MoreThan("محله", 0))
                .Must(x => ValidatorService.MaxLength(x.ToString(), 7)).WithMessage(ValidationMessage.MaxLength("محله", 7));

            RuleFor(x => x.blok)
                .GreaterThan(0).WithMessage(ValidationMessage.MoreThan("بلوک", 0))
                .Must(x => ValidatorService.MaxLength(x.ToString(), 7)).WithMessage(ValidationMessage.MaxLength("بلوک", 7));

            RuleFor(x => x.Melk)
                .GreaterThan(0).WithMessage(ValidationMessage.MoreThan("ملک", 0))
                .Must(x => ValidatorService.MaxLength(x.ToString(), 7)).WithMessage(ValidationMessage.MaxLength("ملک", 7));

            RuleFor(x => x.sakhteman)
                .GreaterThan(0).WithMessage(ValidationMessage.MoreThan("ساختمان", 0))
                .Must(x => ValidatorService.MaxLength(x.ToString(), 7)).WithMessage(ValidationMessage.MaxLength("ساختمان", 7));

            RuleFor(x => x.apar)
                .Equal(0).WithMessage(ValidationMessage.Equal("آپارتمان", "0"));
        });

        RuleSet("aparteman", () =>
        {
            RuleFor(x => x.Index)
                .Must(x => x is >= 2 or <= 4).WithMessage(ValidationMessage.Between("کد نوع پرونده", "2", "4"));

            RuleFor(x => x.shop)
                .GreaterThan(0).WithMessage(ValidationMessage.MoreThan("شماره پرونده والد", 0))
                .Must(x => ValidatorService.MaxLength(x.ToString(), 7)).WithMessage(ValidationMessage.MaxLength("شماره پرونده والد", 10));

            RuleFor(x => x.mantaghe)
                .GreaterThan(0).WithMessage(ValidationMessage.MoreThan("منطقه", 0))
                .Must(x => ValidatorService.MaxLength(x.ToString(), 7)).WithMessage(ValidationMessage.MaxLength("منطقه", 7));

            RuleFor(x => x.hoze)
                .GreaterThan(0).WithMessage(ValidationMessage.MoreThan("محله", 0))
                .Must(x => ValidatorService.MaxLength(x.ToString(), 7)).WithMessage(ValidationMessage.MaxLength("محله", 7));

            RuleFor(x => x.blok)
                .GreaterThan(0).WithMessage(ValidationMessage.MoreThan("بلوک", 0))
                .Must(x => ValidatorService.MaxLength(x.ToString(), 7)).WithMessage(ValidationMessage.MaxLength("بلوک", 7));

            RuleFor(x => x.Melk)
                .GreaterThan(0).WithMessage(ValidationMessage.MoreThan("ملک", 0))
                .Must(x => ValidatorService.MaxLength(x.ToString(), 7)).WithMessage(ValidationMessage.MaxLength("ملک", 7));

            RuleFor(x => x.sakhteman)
                .GreaterThan(0).WithMessage(ValidationMessage.MoreThan("ساختمان", 0))
                .Must(x => ValidatorService.MaxLength(x.ToString(), 7)).WithMessage(ValidationMessage.MaxLength("ساختمان", 7));

            RuleFor(x => x.apar)
                .GreaterThan(0).WithMessage(ValidationMessage.MoreThan("آپارتمان", 0))
                .Must(x => ValidatorService.MaxLength(x.ToString(), 7)).WithMessage(ValidationMessage.MaxLength("آپارتمان", 7));
        });
    }
}