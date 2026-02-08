namespace FormerUrban_Afta.DataAccess.DTOs.IdentityUser;
public class UserLoginedDto : UserLogined
{
    public bool IsValid { get; set; } = true;
    public string ViewerOrLoginer { get; set; }
    public string LoginDateTimeString { get; set; }
    public string LogoutDateTimeString { get; set; }
    public List<string>? message { get; set; }
}

public class UserLoginedSearchDto
{
    public string? UserName { get; set; }
    public string? Ip { get; set; }
    public string? FromDateTime { get; set; }
    public string? ToDateTime { get; set; }    
    public string? ArrivalDate { get; set; }
    public string? DepartureDate { get; set; } 
    public int? ReportType { get; set; } 
}