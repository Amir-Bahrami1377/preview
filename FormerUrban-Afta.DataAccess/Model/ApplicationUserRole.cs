namespace FormerUrban_Afta.DataAccess.Model;
public class ApplicationUserRole : IdentityUserRole<string>
{
    // Add custom fields if needed
    public string Hashed { get; set; }
    public override string ToString()
    {
        return $"{RoleId}{UserId}".Trim();
    }
}