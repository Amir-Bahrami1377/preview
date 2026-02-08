namespace FormerUrban_Afta.DataAccess.DTOs.IdentityUser;
public class CostumIdentityUserDto
{
    [Display(Name = "نام")]
    public string Name { get; set; }

    [Display(Name = "نام خانوادگی")]
    public string Family { get; set; }

    public Guid? Id { get; set; }

    [Display(Name = "نام کاربری")]
    public string UserName { get; set; }

    [Display(Name = "ایمیل")]
    [EmailAddress]
    public string Email { get; set; }

    [Display(Name = "گذرواژه")]
    public string PasswordHash { get; set; }

    [Display(Name = "تلفن")]
    public string PhoneNumber { get; set; }
}