using FormerUrban_Afta.DataAccess.DTOs.Marahel;

namespace FormerUrban_Afta.DataAccess.DTOs.Validation.Marahel
{
    public class TaeedErsalValidator : AbstractValidator<TaeedErsalDto>
    {
        public TaeedErsalValidator()
        {
            RuleFor(x => x.shod)
                .GreaterThan(0).WithMessage("خطا در پردازش شماره درخواست. لطفا مجددا وارد مرحله بشوید!!");

            RuleFor(x => x.shop)
                .GreaterThan(0).WithMessage("خطا در پردازش شماره پرونده. لطفا مجددا وارد مرحله بشوید!!");

            RuleFor(x => x.codeMarhale)
                .GreaterThan(0).WithMessage("خطا در پردازش مرحله بعدی. لطفا مجددا وارد مرحله بشوید!!");

            RuleFor(x => x.marhale)
                .NotEmpty().WithMessage("خطا در پردازش مرحله جاری. لطفا مجددا وارد مرحله بشوید!!");
        }
    }
}
