namespace FormerUrban_Afta.DataAccess.Model;

public class VaultOptions
{
    public string Address { get; set; } = string.Empty;
    public string RoleId { get; set; } = string.Empty;
    public string SecretId { get; set; } = string.Empty;
    public string KeyName { get; set; } = string.Empty;
    public int TokenTTLSeconds { get; set; } = 3600; // Optional
}
