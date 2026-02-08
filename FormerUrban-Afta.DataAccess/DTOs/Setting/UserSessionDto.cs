namespace FormerUrban_Afta.DataAccess.DTOs.Setting
{
    public class UserSessionDto
    {
        public Guid Identity { get; set; } 
        public string Ip { get; set; } 
        public string FullName { get; set; } 
        public string UserAgent { get; set; } 
        public string CreatedAt { get; set; } 
        public string? LastActivity { get; set; }
        public string? ExpiresAt { get; set; }
        public bool IsValid { get; set; } = true;
    }
}
