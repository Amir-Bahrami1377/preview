namespace FormerUrban_Afta.DataAccess.Utilities;

public static class Strings
{
    public static class Persian
    {
        public static class Fields
        {
            #region Errors
            public const string BadLogin = "نام کاربری یا رمز عبور اشتباه است.";
            public const string BadLoginHash = "اطلاعات کاربر نامعتبر است متاسفانه شما تا بررسی اطلاعاتتان توسط مدیر سیستم امکان ورود ندارید.";
            public const string BadLoginRole = "دسترسی شما بر اساس نقش قطع شده است ";
            public const string BadLoginRepeat = "حساب شما به دلیل تلاش های ناموفق زیاد مسدود شده است";
            public const string BadLoginSession = "شما حداکثر نشست فعال خود را استفاده کرده اید";
            public const string BadLoginUserDeactive = "ورود ناموفق به علت غیرفعال بودن کاربر";
            public const string BadLoginSms = "ورود ناموفق کاربر در قسمت ورود پیامکی";
            public const string BadLoginSmsCode = "کد پیامکی وارد شده اشتباه است";
            public const string BadLoginSmsCaptcha = "کد کپچا در قسمت ورود پیامکی اشتباه است";
            public const string BadLoginOtp = "ورود ناموفق کاربر در قسمت ورود OTP";
            //public const string BadLoginOtpCode = "کد OTP وارد شده اشتباه است";
            public const string BadLoginOtpCaptcha = "کد کپچا در قسمت ورود OTP اشتباه است";
            //public const string IllegalAccessError = "دسترسی شما به این بخش مجاز نیست";
            #endregion

            #region SectionNames
            public const string LogIn = "لاگین کاربران";
            public const string LogInOtp = "لاگین دو مرحله ای OTP";
            public const string LogInSms = "لاگین دو مرحله ای SMS";
            public const string LogOut = "خروج کاربران";
            public const string ForceLogout = "خروج کاربر به علت منقضی شدن نشست";
            public const string AccessDeniedLogOut = "خروج کاربر به علت تلاش بیش از حد برای دسترسی های غیر مجاز";
            public const string DataIntegrity = "خروج کاربر به علت خطای صحت داده در نشست";
            public const string roleRestriction = "خروج کاربر به علت مسدود شدن نقش";
            //public const string UserGroups = "گروه کاربران";
            //public const string UsersOfGroup = "کاربران گروه";
            //public const string UserLogIns = "لاگین‌های کاربران";
            //public const string UserDefinition = "تعریف کاربران";
            //public const string UserGrouping = "گروه‌بندی کاربران";
            //public const string UserGroupAccess = "دسترسی به گروه";
            //public const string CartableAccess = "دسترسی به کارتابل";
            //public const string UserAccessLevel = "سطح دسترسی کاربران";
            //public const string UserPanelSettings = "تنظیمات پنل کاربری";
            //public const string PasswordSettings = "ویرایش تنظیمات رمز عبور";
            //public const string LoginSettings = "ویرایش تنظیمات ورود به سامانه";
            //public const string SendingEmailSettings = "ویرایش تنظیمات ارسال ایمیل";
            public const string UserIsDisabled = "تلاش کاربر غیرفعال شده برای ورود به سامانه";
            //public const string ExceedingNotOkLogIns = "حد آستانه برای تعداد ورودهای ناموفق مجاز";
            //public const string ExceedingMaximumSessions = "حد آستانه برای تعداد نشست‌های فعال کاربر";
            //public const string UserInclusionInLoginDateLimit = "شامل شدن کاربر در محدودیت زمانی ورود";
            //public const string UserInclusionInLoginIpLimit = "شامل شدن کاربر در محدودیت آی‌پی یا پورت ورود";
            public const string InvalidHashRole = "خروج کاربر به علت نامعتبر بودن نقش";
            public const string InvalidHashUser = "خروج کاربر به علت نامعتبر بودن اطلاعات کاربر";
            public const string userDeactive = "خروج کاربر به علت غیر فعال بودن کاربر";
            #endregion
        }
    }
}

