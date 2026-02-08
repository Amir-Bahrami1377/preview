namespace FormerUrban_Afta.DataAccess.Model.Enums;

public enum EnumOperation
{
    [Display(Name = "عملیات")]
    None, // 0

    [Display(Name = "نمایش")]
    Get, // 1

    [Display(Name = "ایجاد")]
    Post, // 2

    [Display(Name = "ویرایش")]
    Update, // 3

    [Display(Name = "حذف")]
    Delete, // 4

    [Display(Name = "بررسی صحت سنجی")]
    Validate, // 5
}

public class EnumOperationInfo
{
    public string Name { get; set; }
    public int Index { get; set; }
    public string DisplayName { get; set; }
}

