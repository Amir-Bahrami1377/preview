namespace FormerUrban_Afta.DataAccess.DTOs.IdentityUser;

public record UserDto
{
    public string Id { get; init; }
    public string Name { get; init; }
    public string Family { get; init; }
    public string UserName { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public bool LockoutEnabled { get; set; }
    public bool TwoFactorEnabled { get; set; }
    public bool IsValid { get; set; }
}
