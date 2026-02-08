namespace FormerUrban_Afta.DataAccess.Utilities;

public class ValidationMessage
{
    public static string InvalidSearchType => "کد نوع جستجوی پرونده صحیح نیست!";
    public static string InvalidCodeNosazi => "لطفا کد نوسازی را به صورت صحیح وارد کنید: 7-6-5-4-3-2-1";

    public static string Required(string fieldDisplayName) => $"لطفاً {fieldDisplayName} را وارد کنید!";
    public static string Length(string fieldName, int length) => $"{fieldName} باید {length} رقمی باشد.";
    public static string MaxLength(string fieldName, int length) => $"{fieldName} نمی‌تواند بیشتر از {length} کاراکتر باشد.";
    public static string MinLength(string fieldName, int length) => $"{fieldName} نمی‌تواند کمتر از {length} کاراکتر باشد.";
    public static string OnlyDigits(string fieldName) => $"{fieldName} فقط باید شامل ارقام باشد.";
    public static string MoreThan(string fieldName, int two) => $"{fieldName} باید بزرگتر از {two} باشد.";
    public static string Equal(string fieldName, string two) => $"{fieldName} باید برابر با {two} باشد.";
    public static string Between(string fieldName, string one, string two) => $"{fieldName} باید بین {one} و {two} باشد.";
    public static string SanitizeInput(string fieldName) => $"لطفا از وارد کردن کاراکترهای غیرمجاز در {fieldName} مانند < > \" ' ; % ( ) + = \\ -- خودداری کنید.";
    public static string IsPersianLetters(string fieldName) => $"{fieldName} فقط میتواند شامل حروف فارسی باشد.";
    public static string IsAlphanumeric(string fieldName) => $"{fieldName} فقط میتواند شامل حروف و اعداد باشد.";
    public static string IsValidTel() => $" شماره تلفن باید 8 رقم بدون پیش شماره یا 11 رقم با پیش شماره معتبر باشد (پیش شماره باید با 0 شروع شود و 2 یا 3 رقم باشد).";
    public static string IsValidPostalCode() => "لطفا کد پستی را به درستی وارد کنید. کد پستی باید شامل 10 رقم باشد.";
    public static string ValidAmountFormat(string fieldName) => $"لطفاً {fieldName} را با فرمت صحیح وارد کنید (حداکثر دو رقم اعشار مجاز است).";
    public static string AmountLessThanOrEqualToMax(string fieldName, string max) => $"{fieldName} نباید از {max} بیشتر باشد.";
    public static string ValidUtmX() => "مختصات X باید بین ۲۰۰٬۰۰۰ تا ۹۰۰٬۰۰۰ باشد.";
    public static string ValidUtmY() => "مختصات Y باید بین ۳٬3۰۰٬۰۰۰ تا ۴٬۲۰۰٬۰۰۰ باشد.";
    public static string IsValidNationalCode() => "کد ملی معتبر نیست!";
    public static string IsValidLegalNationalId() => "شناسه ملی معتبر نیست!";
    public static string IsValidMobileNumber() => "شماره همراه وارد شده معتبر نیست!";
    public static string IsValidEmail() => "ایمیل وارد شده معتبر نیست!";
    public static string IsValidPersianDate(string fieldName) => $"فرمت صحیح {fieldName}: YYYY/MM/DD";
    public static string IsValidInsuranceNumber() => $"شماره بیمه کارگران باید 8 رقمی باشد!";
    public static string IsValidIpRangeOrCidr() => $"آی پی نامعتبر است.";

    public static string IsValidMinPassword() => $"رمز عبور باید بیشتر از 12 کارکتر باشد!";
    public static string IsValidMaxPassword() => $"رمز عبور باید کمتر از 128 کارکتر باشد!";
    public static string IsValidScorePassword() => $"کاربر گرامی حداقل قدرت پسور باید خوب باشد تا مورد تایید قرار بگیرد!";
    public static string RepeatPassword() => $"رمز عبور و تکرار رمز عبور با هم برابر نیست!";
    public static string RepeatPassword2() => $"رمز عبور قدیم و رمز عبور جدید نمیتواند با هم برابر باشند!";
    public static string MobilePassword() => $"شماره موبایل و رمز عبور جدید نمیتواند با هم برابر باشند!";
    public static string Range(string field, int min, int max) => $"{field} باید عددی بین {min} و {max} باشد.";
}