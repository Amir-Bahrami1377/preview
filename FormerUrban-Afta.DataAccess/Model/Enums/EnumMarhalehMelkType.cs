namespace FormerUrban_Afta.DataAccess.Model.Enums;

public enum EnumMarhalehMelkType
{
    [Display(Name = " ")]
    None,  //0

    [Display(Name = "اتمام عملیات")]
    finished, //1

    [Display(Name = "در حد سفت کاری")]
    seftkari, //2

    [Display(Name = "غیر قابل استفاده")]
    unusable, //3

    [Display(Name = "در مرحله پی کنی و پی سازی")]
    peykaniosazi, //4

    [Display(Name = "در مرحله اسکلت")]
    esk, //5

    [Display(Name = "در مرحله اسکلت و سقف")]
    eskosakh, //6

    [Display(Name = "زمین محصور")]
    zaminmahsour, //7

    [Display(Name = "زمین بایر")]
    zaminbayer, //8

    [Display(Name = "ساختمان مخروبه")]
    sakhmakhrub, //9
}

