using FormerUrban_Afta.DataAccess.Services;

namespace FormerUrban_Afta.DataAccess.DTOs.Validation.Setting
{
    public class EventLogThresholdValidation : AbstractValidator<EventLogThresholdDto>
    {
        public EventLogThresholdValidation()
        {
            RuleFor(x => Convert.ToInt32(x.UsersLoginLogWarning))
                .GreaterThanOrEqualTo(1000).WithMessage(ValidationMessage.MoreThan("حد آستانه ی هشدار برای حجم جدول ذخیره سازی لاگ ورود/خروج کاربران (مگابایت)", 1000));

            RuleFor(x => Convert.ToInt32(x.UsersLoginLogCritical))
                .GreaterThanOrEqualTo(1100).WithMessage(ValidationMessage.MoreThan("حد آستانه ی بحرانی برای حجم جدول ذخیره سازی لاگ ورود/خروج کاربران (مگابایت)", 1100));

            RuleFor(x => x)
                .Must(x => Convert.ToInt32(x.UsersLoginLogWarning) < Convert.ToInt32(x.UsersLoginLogCritical)).WithMessage("مقدار حد آستانهی هشدار برای حجم جدول ذخیرهسازی لاگ ورود/خروج کاربران باید کمتر از مقدار حد آستانه ی بحرانی برای حجم جدول ذخیرهسازی لاگ ورود/خروج کاربران باشد.");


            RuleFor(x => Convert.ToInt32(x.UsersActivityLogWarning))
                .GreaterThanOrEqualTo(1000).WithMessage(ValidationMessage.MoreThan("حد آستانه ی هشدار برای حجم جدول ذخیره سازی لاگ فعالیت کاربران (مگابایت)", 1000));

            RuleFor(x => Convert.ToInt32(x.UsersActivityLogCritical))
                .GreaterThanOrEqualTo(1100).WithMessage(ValidationMessage.MoreThan("حد آستانه ی بحرانی برای حجم جدول ذخیره سازی لاگ فعالیت کاربران (مگابایت)", 1100));

            RuleFor(x => x)
                .Must(x => Convert.ToInt32(x.UsersActivityLogWarning) < Convert.ToInt32(x.UsersActivityLogCritical)).WithMessage("مقدار حد آستانهی هشدار برای حجم جدول ذخیرهسازی لاگ فعالیت کاربران باید کمتر از مقدار حد آستانهی بحرانی برای حجم جدول ذخیره سازی لاگ فعالیت کاربران باشد.");


            RuleFor(x => Convert.ToInt32(x.UsersAuditsLogWarning))
                .GreaterThanOrEqualTo(1000).WithMessage(ValidationMessage.MoreThan("حد آستانهی هشدار برای حجم جدول ذخیرهسازی لاگ تغییرات داده های سیستم (مگابایت)", 1000));

            RuleFor(x => Convert.ToInt32(x.UsersAuditsLogCritical))
                .GreaterThanOrEqualTo(1100).WithMessage(ValidationMessage.MoreThan("حد آستانهی بحرانی برای حجم جدول ذخیرهسازی لاگ تغییرات داده های سیستم (مگابایت)", 1100));

            RuleFor(x => x)
                .Must(x => Convert.ToInt32(x.UsersAuditsLogWarning) < Convert.ToInt32(x.UsersAuditsLogCritical)).WithMessage("مقدار حد آستانهی هشدار برای حجم جدول ذخیره سازی لاگ تغییرات داده های سیستم باید کمتر از مقدار حد آستانه ی بحرانی برای حجم جدول ذخیره سازی لاگ تغییرات داده های سیستم باشد.");


            RuleFor(x => x.UserId)
                .MaximumLength(1000).WithMessage(ValidationMessage.MaxLength("ارسال پیامک به کاربر", 1000))
                .Must(ValidatorService.SanitizeAndValidateInput).WithMessage(ValidationMessage.SanitizeInput("ارسال پیامک به کاربر"))
                .When(x => !string.IsNullOrWhiteSpace(x.UserId));

        }
    }
}
