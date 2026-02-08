namespace FormerUrban_Afta.DataAccess.Model.Enums;

public enum EnumMarhalehType
{
    [Display(Name = " ")]
    None,  //0

    [Display(Name = "بازدید ")]
    Visit, //1

    [Display(Name = "کنترل نقشه")]
    ControlMap, //2

    [Display(Name = "پاسخ استعلام")]
    InquiryResponse, //3

    [Display(Name = "صدور پروانه")]
    IssuanceLicense, //4

    [Display(Name = "بایگانی")]
    Archive, //5
}
public class EnumMarhalehTypeInfo
{
    public string Name { get; set; }
    public int Index { get; set; }
    public string DisplayName { get; set; }
}
