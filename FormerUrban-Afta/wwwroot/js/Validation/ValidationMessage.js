const ValidationMessage = {

    InvalidSearchType: "کد نوع جستجوی پرونده صحیح نیست!",
    InvalidCodeNosazi: "لطفا کد نوسازی را به صورت صحیح وارد کنید: 7-6-5-4-3-2-1",

    Required: function (fieldName) {
        return `لطفاً ${fieldName} را وارد کنید!`;
    },

    Length: function (fieldName, length) {
        return `${fieldName} باید ${length} رقمی باشد!`;
    },

    MinLength: function (fieldName, length) {
        return `${fieldName} نمی‌تواند کمتر از ${length} کاراکتر باشد.`;
    },

    MaxLength: function (fieldName, length) {
        return `${fieldName} نمی‌تواند بیشتر از ${length} کاراکتر باشد.`;
    },

    MaxNumberLength: function (fieldName, length) {
        return `${fieldName} نمی‌تواند بیشتر از ${length} باشد.`;
    },

    Equal: function (fieldName, value) {
        return `${fieldName} باید برار با ${value} باشد.`;
    },

    NotEqual: function (fieldName, value) {
        return `${fieldName} نمیتواند برار با ${value} باشد.`;
    },

    OnlyDigits: function (fieldName) {
        return `${fieldName} فقط باید شامل ارقام باشد.`;
    },

    MoreThan: function (max, fieldName) {
        return `${fieldName} باید بزرگتر از ${max} باشد.`;
    },

    IsPersianLetters: function (fieldName) {
        return `${fieldName} فقط میتواند شامل حروف فارسی باشد.`;
    },

    isAlphanumeric: function (fieldName) {
        return `${fieldName} فقط میتواند شامل حروف و اعداد باشد.`;
    },

    SanitizeAndValidateInput: function (fieldName) {
        return `لطفا از وارد کردن کاراکترهای غیرمجاز در ${fieldName} مانند < > \" ' ; % ( ) + = \\ --  یا کد های Sql خودداری کنید.`;
    },

    Between: function (fieldName, one, two) {
        return `${fieldName} باید بین ${one} و ${two} باشد.`;
    },

    IsValidTel: function (fieldName) {
        return `شماره تلفن باید 8 رقم بدون پیش شماره یا 11 رقم با پیش شماره معتبر باشد (پیش شماره باید با 0 شروع شود و 2 یا 3 رقم باشد).`
    },

    IsValidPostalCode: function (fieldName) {
        return `لطفا کد پستی را به درستی وارد کنید. کد پستی باید شامل 10 رقم باشد.`
    },

    ValidAmountFormat: function (fieldName) {
        return `لطفاً ${fieldName} را با فرمت صحیح وارد کنید (حداکثر دو رقم اعشار (/) مجاز است).`;
    },

    IsValidUtmX: function () {
        return "مختصات X باید بین ۲۰۰٬۰۰۰ تا ۹۰۰٬۰۰۰ باشد.";
    },

    IsValidUtmY: function () {
        return "مختصات Y باید بین ۳٬3۰۰٬۰۰۰ تا ۴٬۲۰۰٬۰۰۰ باشد.";
    },

    IsValidNationalCodeLength: function () {
        return "کد ملی باید دقیقاً 10 رقم باشد";
    },

    IsValidNationalCodeRepet: function () {
        return "کد ملی نمی‌تواند شامل 10 رقم تکراری باشد";
    },

    IsValidNationalCode: function () {
        return "کد ملی نامعتبر است";
    },

    IsValidMobileNumber: function () {
        return "شماره همراه نامعتبر است";
    },

    IsValidEmail: function () {
        return "ایمیل وارد شده معتبر نیست";
    },

    IsValidPersianDate: function (fieldName) {
        return `فرمت صحیح ${fieldName}: YYYY/MM/DD`;
    },

    IsValidPersianDateYearEmpty: function (fieldName) {
        return `مقدار سال در ${fieldName} نمی تواند خالی باشد.`;
    },

    IsValidPersianDateMonthEmpty: function (fieldName) {
        return `مقدار ماه در ${fieldName} نمی تواند خالی باشد.`;
    },

    IsValidPersianDateDayEmpty: function (fieldName) {
        return `مقدار روز در ${fieldName} نمی تواند خالی باشد.`;
    },

    IsValidPersianDateYear: function (fieldName) {
        return `مقدار سال در ${fieldName} باید بین 1300 تا 1499 باشد.`;
    },

    IsValidPersianDateMonth: function (fieldName) {
        return `مقدار ماه در ${fieldName} باید بین 1 تا 12 باشد.`;
    },

    IsValidPersianDateDay: function (fieldName) {
        return `مقدار روز در ${fieldName} باید بزرگتر از 0 باشد.`;
    },

    IsValidPersianDateDay2: function (fieldName, day) {
        return `حداکثر روز در ${fieldName} برابر با ${day} است.`;
    },

    IsValidBirthCertificateNumber: function () {
        return `شماره شناسنامه باید فقط عدد و حداکثر ۱۰ رقم باشد.`;
    },

    IsValidLegalNationalId: function () {
        return `شناسه ملی معتبر نیست`;
    },

    IsValidLegalNationalIdLength: function () {
        return `شناسه ملی باید 11 رقم باشد`;
    },

    isValidIpRangeOrCidr: function () {
        return `آی پی نامعتبر است.`;
    },

    isValidPersianDateTime: function (filedName) {
        return `فرمت صحیح ${fieldName}: YYYY/MM/DD HH:mm:ss`;
    },

    IsValidPersianDateHourEmpty: function (fieldName) {
        return `مقدار ساعت در ${fieldName} نمی تواند خالی باشد.`;
    },

    IsValidPersianDateMinuteEmpty: function (fieldName) {
        return `مقدار دقیقه در ${fieldName} نمی تواند خالی باشد.`;
    },

    IsValidPersianDateSecondEmpty: function (fieldName) {
        return `مقدار ثانیه در ${fieldName} نمی تواند خالی باشد.`;
    },

    IsValidPersianDateHour: function (fieldName) {
        return `مقدار ساعت در ${fieldName} باید بین 1300 تا 1499 باشد.`;
    },

    IsValidPersianDateMinute: function (fieldName) {
        return `مقدار دقیقه در ${fieldName} باید بین 1 تا 12 باشد.`;
    },

    IsValidPersianDateSecond: function (fieldName) {
        return `مقدار ثانیه در ${fieldName} باید بزرگتر از 0 باشد.`;
    },

    isValidIp: function () {
        return `آیپی نامعتبر است!`;
    },

    isValidpassword: function () {
        return `کاربر گرامی حداقل قدرت رمز عبور باید خوب باشد تا مورد تایید قرار بگیرد!`;
    },
};

window.ValidationMessage = ValidationMessage;
