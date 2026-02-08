namespace FormerUrban_Afta.DataAccess.Model.Enums
{
    public enum EnumFormName
    {
        [Display(Name = "نام فرم")]
        None,  //0

        [Display(Name = "آپارتمان")]
        Apartman, //1

        [Display(Name = "آیپی های مجاز")]
        AllowedIPRange, //2

        [Display(Name = "بایگانی")]
        Baygani, //3

        [Display(Name = "آیپی های بلاک شده")]
        BlockedIPRange, //4

        [Display(Name = "کنترل نقشه")]
        ControlMap, //5

        [Display(Name = "درخواست")]
        Darkhast, //6

        [Display(Name = "کاربری")]
        Dv_karbari, //7

        [Display(Name = "مالکین")]
        Dv_malekin, //8

        [Display(Name = "سوابق")]
        Dv_savabegh, //9

        [Display(Name = "ارجاع")]
        Erja, //10

        [Display(Name = "استعلام")]
        Estelam, //11

        [Display(Name = "کارشناس بازدید")]
        Expert, //12

        [Display(Name = "ملک")]
        Melk, //13

        [Display(Name = "پرونده")]
        Parvandeh, //14

        [Display(Name = "پروانه")]
        Parvaneh, //15

        [Display(Name = "دسترسی نقش")]
        RolePermission, //16

        [Display(Name = "ساختمان")]
        Sakhteman, //17

        [Display(Name = "تعریف ها")]
        Tarifha, //18

        [Display(Name = "دسترسی کاربر")]
        UserPermission, //19

        [Display(Name = "نشست های کاربر")]
        UserSession, //20

        [Display(Name = "آستانه ثبت رویداد")]
        EventLogThreshold, //21

        [Display(Name = "محدود کردن بر اساس نقش")]
        RoleRestriction, //22

        [Display(Name = "مدیریت کاربران")]
        AspNetUsers, // 23

        [Display(Name = "مرحله بازدید")]
        Visit, // 24

        [Display(Name = "گزارشات عادی")]
        History, // 25

        [Display(Name = "فیلتر رویداد های ممیزی")]
        EventLogFilter, // 26

        [Display(Name = "لاگین های کاربران")]
        UserLogined, // 27

        [Display(Name = "گزارشات پیام کوتاه")]
        LogSMS, // 28

        [Display(Name = "گزارشات فعال")]
        ActivityLogFilters, // 29

        [Display(Name = "سرویس رمزنگاری")]
        Vault, // 30

        [Display(Name = "گزارشات تغییرات")]
        Audit, // 31

        [Display(Name = "مدیریت گزارشات تغییرات")]
        AuditFilters, // 31
    }

    public class EnumFormNameInfo
    {
        public string Name { get; set; }
        public int Index { get; set; }
        public string DisplayName { get; set; }
    }
}
