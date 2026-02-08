namespace FormerUrban_Afta.DataAccess.DTOs.IdentityUser;
public class CreateCostumIdentityUserDto
{
    public string? OldUserName { get; set; }

    public string Name { get; set; }

    public string Family { get; set; }

    public string UserName { get; set; }

    public string Email { get; set; }

    public string PasswordHash { get; set; }

    public string RepeatPassword { get; set; }

    public string PhoneNumber { get; set; }

    public string[] Role { get; set; }

    public List<CostumIdentityRole>? Roles { get; set; }
}