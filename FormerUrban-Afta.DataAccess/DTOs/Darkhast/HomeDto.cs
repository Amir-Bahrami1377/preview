using FormerUrban_Afta.DataAccess.DTOs.IdentityUser;

namespace FormerUrban_Afta.DataAccess.DTOs.Darkhast;

public class HomeDto
{
    public List<KartableDTO> kartable { get; set; }
    public List<UserLoginedDto> Success { get; set; }
    public List<UserLoginedDto> Failed { get; set; }
}

