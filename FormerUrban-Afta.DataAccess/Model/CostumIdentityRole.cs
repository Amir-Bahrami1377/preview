namespace FormerUrban_Afta.DataAccess.Model;
public class CostumIdentityRole : IdentityRole
{
    public string Description { get; set; }
    public string Hashed { get; set; }
    public override string ToString()
    {
        return $"{Id}{Description}{Name}{NormalizedName}{ConcurrencyStamp}".Trim();
    }
}
