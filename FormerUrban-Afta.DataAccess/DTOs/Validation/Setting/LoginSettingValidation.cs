using FormerUrban_Afta.DataAccess.Services;

namespace FormerUrban_Afta.DataAccess.DTOs.Validation.Setting
{
    public class LoginSettingValidation : AbstractValidator<LoginSettingDto>
    {
        public LoginSettingValidation()
        {
            //RuleFor(x => x.MaximumSessions)
            //    .NotEmpty().WithMessage(ValidationMessage.Required("حداکثر تعداد نشست"))
            //    .GreaterThan(0).WithMessage(ValidationMessage.Required("حداکثر تعداد نشست"));

            RuleFor(x => x.MaximumSessions)
                .Must(ValidatorService.SanitizeAndValidateInput)
                .WithMessage(ValidationMessage.SanitizeInput("حداکثر تعداد نشست"))
                .Must(ValidatorService.IsDigitsOnly).WithMessage(ValidationMessage.OnlyDigits("حداکثر تعداد نشست"))
                .When(x => !string.IsNullOrWhiteSpace(x.MaximumSessions))
                .NotEmpty().WithMessage(ValidationMessage.Required("حداکثر تعداد نشست"))
                .NotEqual("0").WithMessage(ValidationMessage.MoreThan("حداکثر تعداد نشست", 0))
                //.Length(1).WithMessage(ValidationMessage.Equal("حداکثر تعداد نشست", "1 کارکتر"))
                .Must(value =>
                {
                    if (int.TryParse(value, out int number))
                        return number is >= 1 and <= 10;
                    return false;
                }).WithMessage(ValidationMessage.Range("حداکثر تعداد نشست", 1, 10));

            //RuleFor(x => x.RetryLoginCount)
            //    .NotEmpty().WithMessage(ValidationMessage.Required("تلاش برای لاگین ناموفق"))
            //    .GreaterThan(0).WithMessage(ValidationMessage.Required("تلاش برای لاگین ناموفق"));

            RuleFor(x => x.RetryLoginCount)
                .Must(ValidatorService.SanitizeAndValidateInput)
                .WithMessage(ValidationMessage.SanitizeInput("تلاش برای لاگین ناموفق"))
                .Must(ValidatorService.IsDigitsOnly).WithMessage(ValidationMessage.OnlyDigits("تلاش برای لاگین ناموفق"))
                .When(x => !string.IsNullOrWhiteSpace(x.RetryLoginCount))
                .NotEmpty().WithMessage(ValidationMessage.Required("تلاش برای لاگین ناموفق"))
                .NotEqual("0").WithMessage(ValidationMessage.MoreThan("تلاش برای لاگین ناموفق", 0))
                //.Length(1).WithMessage(ValidationMessage.Equal("تلاش برای لاگین ناموفق", "1 کارکتر"))
                .Must(value =>
                {
                    if (int.TryParse(value, out int number))
                        return number is >= 1 and <= 10;
                    return false;
                }).WithMessage(ValidationMessage.Range("تلاش برای لاگین ناموفق", 1, 10));

            //RuleFor(x => x.KhatemeSessionAfterMinute)
            //    .NotEmpty().WithMessage(ValidationMessage.Required("خاتمه نشست (دقیقه)"))
            //    .GreaterThan(0).WithMessage(ValidationMessage.Required("خاتمه نشست (دقیقه)"));

            RuleFor(x => x.KhatemeSessionAfterMinute)
                .Must(ValidatorService.SanitizeAndValidateInput)
                .WithMessage(ValidationMessage.SanitizeInput("خاتمه نشست (دقیقه)"))
                .Must(ValidatorService.IsDigitsOnly).WithMessage(ValidationMessage.OnlyDigits("خاتمه نشست (دقیقه)"))
                .When(x => !string.IsNullOrWhiteSpace(x.KhatemeSessionAfterMinute))
                .NotEmpty().WithMessage(ValidationMessage.Required("خاتمه نشست (دقیقه)"))
                .NotEqual("0").WithMessage(ValidationMessage.MoreThan("خاتمه نشست (دقیقه)", 0))
                //.MinimumLength(2).WithMessage(ValidationMessage.Equal("خاتمه نشست (دقیقه)", "2 حداقل کارکتر"))
                .MaximumLength(3).WithMessage(ValidationMessage.Equal("خاتمه نشست (دقیقه)", "3 حد اکثر کارکتر"))
                .Must(value =>
                {
                    if (int.TryParse(value, out int number))
                        return number >= 1;
                    return false;
                }).WithMessage(ValidationMessage.MoreThan("خاتمه نشست (دقیقه)", 0));

            RuleFor(x => x.MaximumAccessDenied)
                .Must(ValidatorService.SanitizeAndValidateInput)
                .WithMessage(ValidationMessage.SanitizeInput("حداکثر تعداد درخواست دسترسی رد شده"))
                .Must(ValidatorService.IsDigitsOnly).WithMessage(ValidationMessage.OnlyDigits("حداکثر تعداد درخواست دسترسی رد شده"))
                .When(x => !string.IsNullOrWhiteSpace(x.MaximumAccessDenied))
                .NotEmpty().WithMessage(ValidationMessage.Required("حداکثر تعداد درخواست دسترسی رد شده"))
                .NotEqual("0").WithMessage(ValidationMessage.MoreThan("حداکثر تعداد درخواست دسترسی رد شده", 0))
                //.Length(1).WithMessage(ValidationMessage.Equal("حداکثر تعداد درخواست دسترسی رد شده", "1 کارکتر"))
                .Must(value =>
                {
                    if (int.TryParse(value, out int number))
                        return number is > 1 and < 10;
                    return false;
                }).WithMessage(ValidationMessage.Range("حداکثر تعداد درخواست دسترسی رد شده", 1, 10));

            RuleFor(x => x.RequestRateLimitter)
                .Must(ValidatorService.SanitizeAndValidateInput)
                .WithMessage(ValidationMessage.SanitizeInput("حداکثر تعداد درخواست در 30 ثانیه"))
                .Must(ValidatorService.IsDigitsOnly)
                .WithMessage(ValidationMessage.OnlyDigits("حداکثر تعداد درخواست در 30 ثانیه"))
                .When(x => !string.IsNullOrWhiteSpace(x.RequestRateLimitter))
                .NotEmpty().WithMessage(ValidationMessage.Required("حداکثر تعداد درخواست در 30 ثانیه"))
                .NotEqual("0").WithMessage(ValidationMessage.MoreThan("حداکثر تعداد درخواست در 30 ثانیه", 0))
                .Must(value =>
                {
                    if (int.TryParse(value, out int number))
                        return number is >= 10 and <= 200;
                    return false;
                }).WithMessage(ValidationMessage.Range("حداکثر تعداد درخواست در 30 ثانیه", 10, 200));
            //.MinimumLength(2).WithMessage(ValidationMessage.Equal("حداکثر تعداد درخواست در 30 ثانیه", "2 حداقل کارکتر"))
            //.MaximumLength(3).WithMessage(ValidationMessage.Equal("حداکثر تعداد درخواست در 30 ثانیه", "3 حد اکثر کارکتر"))


            //RuleFor(x => x.PasswordLength)
            //    .NotEmpty().WithMessage(ValidationMessage.Required("طول رمز عبور"))
            //    .InclusiveBetween(12, 128).WithMessage(ValidationMessage.Between("طول رمز عبور", "12", "128"));

            RuleFor(x => x.PasswordLength)
                .Must(ValidatorService.SanitizeAndValidateInput)
                .WithMessage(ValidationMessage.SanitizeInput("طول رمز عبور"))
                .Must(ValidatorService.IsDigitsOnly).WithMessage(ValidationMessage.OnlyDigits("طول رمز عبور"))
                .When(x => !string.IsNullOrWhiteSpace(x.PasswordLength))
                .NotEmpty().WithMessage(ValidationMessage.Required("طول رمز عبور"))
                .NotEqual("0").WithMessage(ValidationMessage.MoreThan("طول رمز عبور", 0))
                .NotEqual("00").WithMessage(ValidationMessage.MoreThan("طول رمز عبور", 0))
                //.MinimumLength(2).WithMessage(ValidationMessage.Equal("طول رمز عبور", "2 حداقل کارکتر"))
                //.MaximumLength(3).WithMessage(ValidationMessage.Equal("طول رمز عبور", "3 حداکثر کارکتر"))
                // مقدار عددی بعد از تبدیل به int باید بین 12 تا 128 باشد
                .Must(value =>
                {
                    if (int.TryParse(value, out int number))
                        return number is >= 12 and <= 128;
                    return false;
                })
                .WithMessage(ValidationMessage.Range("طول رمز عبور", 12, 128));

            RuleFor(x => x.UnblockingUserTime)
                .NotEmpty().WithMessage(ValidationMessage.Required("رفع مسدود بودن کاربر"))
                .Must(ValidatorService.SanitizeAndValidateInput).WithMessage(ValidationMessage.SanitizeInput("رفع مسدود بودن کاربر"))
                .Must(ValidatorService.IsDigitsOnly).WithMessage(ValidationMessage.OnlyDigits("رفع مسدود بودن کاربر"))
                .Must(x =>
                {
                    if (int.TryParse(x, out int time))
                        return time is > 0 and <= 525600;
                    return false;
                })
                .WithMessage(ValidationMessage.Range("رفع مسدود بودن کاربر", 1, 525600));
        }
    }
}
