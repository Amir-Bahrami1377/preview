namespace FormerUrban_Afta.DataAccess.DTOs.Login;
public class ReAuthenticateDto
{
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    public string ReturnUrl { get; set; }
}
