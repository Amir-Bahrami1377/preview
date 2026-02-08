namespace FormerUrban_Afta.DataAccess.Model.Enums;

public enum EnumPermission
{
    #region منو

    [Display(Name = "منو : کارتابل")]
    Menu_Kartabl = 0,

    [Display(Name = "منو : تغییر رمز عبور کاربر")]
    UserChangePassword = 1,

    #endregion

    #region پرونده

    [Display(Name = "پرونده : اطلاعات پرونده")]
    Menu_ParvandehInfo = 2,

    [Display(Name = "اطلاعات پرونده : ایجاد پرونده")]
    Parvandeh_CreateParvandeh = 3,

    [Display(Name = "اطلاعات پرونده : اطلاعات ملک")]
    Parvandeh_MelkDetail = 4,

    [Display(Name = "اطلاعات پرونده : اطلاعات ساختمان")]
    Parvandeh_SakhtemanDetail = 5,

    [Display(Name = "اطلاعات پرونده : اطلاعات آپارتمان")]
    Parvandeh_ApartemanDetail = 6,

    [Display(Name = "اطلاعات ملک : ویرایش اطلاعات")]
    Melk_Edit = 7,

    [Display(Name = "اطلاعات ساختمان : ویرایش اطلاعات")]
    Sakhteman_Edit = 8,

    [Display(Name = "اطلاعات آپارتمان : ویرایش اطلاعات")]
    Aparteman_Edit = 9,

    [Display(Name = "اطلاعات پرونده : سوابق")]
    Parvandeh_Savabegh = 10,

    [Display(Name = "اطلاعات پرونده : کاربری ها")]
    Parvandeh_Karbari = 11,

    [Display(Name = "اطلاعات پرونده : مالکین")]
    Parvandeh_Malekin = 12,

    [Display(Name = "مالکین : ایجاد مالک")]
    Malekin_Create = 13,

    [Display(Name = "مالکین : ویرایش مالک")]
    Malekin_Edit = 14,

    [Display(Name = "مالکین : حذف مالک")]
    Malekin_Delete = 15,

    [Display(Name = "کاربری : ایجاد کاربری")]
    Karbari_Create = 16,

    [Display(Name = "کاربری : ویرایش کاربری")]
    Karbari_Edit = 17,

    [Display(Name = "کاربری : حذف کاربری")]
    Karbari_Delete = 18,

    [Display(Name = "پرونده : جستجوی پرونده")]
    Menu_SearchParvandeh = 19,

    [Display(Name = "جستجوی پرونده : ثبت درخواست")]
    SearchParvandeh_SabtDarkhast = 20,

    [Display(Name = "درخواست : مشاهده جزئیات درخواست")]
    Darkhast_Detaile = 21,

    [Display(Name = "پرونده : بایگانی")]
    Menu_Baygani = 22,


    #endregion

    #region کاربران

    [Display(Name = "کاربران : مدیریت کاربران")]
    Menu_User = 23,

    [Display(Name = "کاربران : دسترسی کاربران")]
    Menu_UserPermission = 24,

    [Display(Name = "کاربران : دسترسی نقش ها")]
    Menu_RolePermission = 25,

    [Display(Name = "مدیریت کاربران : ایجاد کاربر")]
    UserManage_CreateUser = 26,

    [Display(Name = "مدیریت کاربران : ویرایش کاربر")]
    UserManage_EditUser = 27,

    [Display(Name = "مدیریت کاربران : مسدود کردن کاربر")]
    UserManage_BlockUser = 28,

    [Display(Name = "مدیریت کاربران : تایید دو مرحله ای")]
    EnableAuthenticator = 29,

    [Display(Name = "دسترسی کاربران : ویرایش اطلاعات")]
    UserPermission_Edit = 30,

    [Display(Name = "دسترسی نقش ها : ویرایش اطلاعات")]
    RolePermission_Eidt = 31,

    #endregion

    #region مدیریت نرم افزار

    [Display(Name = "مدیریت نرم افزار : مدیریت پنل پیامکی")]
    Menu_SmsSetting = 32,

    [Display(Name = "مدیریت نرم افزار : مدیریت لاگین")]
    Menu_LoginSetting = 33,

    [Display(Name = "مدیریت نرم افزار : IP مسدود شده")]
    Menu_IPBlockList = 34,

    [Display(Name = "مدیریت نرم افزار : IP مجاز دسترسی")]
    Menu_IPWhiteList = 35,

    [Display(Name = "مدیریت نرم افزار : فیلتر رویداد ها ممیزی")]
    Menu_EventLogFilter = 36,

    [Display(Name = "مدیریت نرم افزار : حد آستانه رویداد ها ممیزی")]
    Menu_EventLogThreshold = 37,

    [Display(Name = "مدیریت نرم افزار : نقش های مسدود شده")]
    Menu_RoleRestriction = 38,

    [Display(Name = "مدیریت نرم افزار : گزارشات فعال")]
    Menu_ActivityLogFilters = 39,

    [Display(Name = "مدیریت نرم افزار : فیلتر تغییرات")]
    Menu_AuditFilters = 40,

    [Display(Name = "مدیریت پنل پیامکی : ویرایش اطلاعات")]
    SmsSetting_Edit = 41,

    [Display(Name = "مدیریت لاگین : ویرایش اطلاعات")]
    LoginSetting_Edit = 42,

    [Display(Name = "مدیریت IP مسدود شده : ایجاد رنج IP")]
    IpBlock_Create = 43,

    [Display(Name = "مدیریت IP مسدود شده : ویرایش رنج IP")]
    IpBlock_Edit = 44,

    [Display(Name = "مدیریت IP مسدود شده : حذف رنج IP")]
    IpBlock_Delete = 45,

    [Display(Name = "مدیریت IP مجاز دسترسی: ایجاد رنج IP")]
    IpWhite_Create = 46,

    [Display(Name = "مدیریت IP مجاز دسترسی : ویرایش رنج IP")]
    IpWhite_Edit = 47,

    [Display(Name = "مدیریت IP مجاز دسترسی : حذف رنج IP")]
    IpWhite_Delete = 48,

    [Display(Name = "فیلتر رویداد ها ممیزی : ویرایش اطلاعات")]
    EventLogFilter_Edit = 49,

    [Display(Name = "حد آستانه رویداد های ممیزی : ویرایش اطلاعات")]
    EventLogThreshold_Edit = 50,

    [Display(Name = "نقش های مسدود شده : ایجاد نقش مسدود شده")]
    RoleRestriction_Create = 51,

    [Display(Name = "نقش های مسدود شده : ویرایش نقش مسدود شده")]
    RoleRestriction_Edit = 52,

    [Display(Name = "نقش های مسدود شده : حذف نقش مسدود شده")]
    RoleRestriction_Delete = 53,

    [Display(Name = "گزارشات فعال : ایجاد")]
    ActivityLogFilters_Create = 54,

    [Display(Name = "گزارشات فعال : ویرایش")]
    ActivityLogFilters_Edit = 55,

    [Display(Name = "گزارشات فعال : حذف")]
    ActivityLogFilters_Delete = 56,

    [Display(Name = "فیلتر تغییرات : ایجاد")]
    AuditFilters_Create = 57,

    [Display(Name = "فیلتر تغییرات : حذف")]
    AuditFilters_Delete = 58,

    #endregion

    #region گزارشات نرم افزار

    [Display(Name = "گزارشات نرم افزار : لاگین های کاربران")]
    Menu_UserLogined = 59,

    [Display(Name = "گزارشات نرم افزار : نشست های کاربران")]
    Menu_Account = 60,

    [Display(Name = "گزارشات نرم افزار : گزارشات عادی")]
    Reports_Regular = 61,

    [Display(Name = "گزارشات نرم افزار : گزارشات پیامک")]
    Reports_Sms = 62,

    [Display(Name = "گزارشات نرم افزار : گزارشات تغییرات")]
    Reports_Audit = 63,

    [Display(Name = "نشست های کاربران : حذف نشست های کاربران")]
    Reports_DeleteUserSession = 64,

    #endregion

    #region مراحل

    [Display(Name = "مراحل : تایید و ارسال")]
    TaeedErsal = 65,

    [Display(Name = "مرحله بازدید : اطلاعات مامور بازدید")]
    Visit_Expert = 66,

    [Display(Name = "مرحله بازدید : ایجاد مامور بازدید")]
    Visit_CreateExpert = 67,

    [Display(Name = "مرحله بازدید : ویرایش مامور بازدید")]
    Visit_EditExpert = 68,

    [Display(Name = "مرحله بازدید : حذف مامور بازدید")]
    Visit_DeleteExpert = 69,

    [Display(Name = "مرحله کنترل نقشه : ویرایش اطلاعات")]
    ControlMap_Edit = 70,

    [Display(Name = "مرحله پاسخ استعلام : ویرایش اطلاعات")]
    Estelam_Edit = 71,

    [Display(Name = "مرحله صدور پروانه : ویرایش اطلاعات")]
    Parvaneh_Edit = 72,

    #endregion

}
