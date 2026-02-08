namespace FormerUrban_Afta.DataAccess.DTOs.Marahel;
public class ExpertDto
{
    public long Identity { get; set; }

    [Required(ErrorMessage = "لطفا نام را وارد کنید!")]
    public string Name { get; set; }

    [Required(ErrorMessage = "لطفا نام خانوادگی را وارد کنید!")]
    public string Family { get; set; }

    public int RequestNumber { get; set; }

    [Required(ErrorMessage = "لطفا تاریخ بازدید را انتخاب کنید!")]
    public string? DateVisit { get; set; }

    public bool IsValid { get; set; } = true;
}