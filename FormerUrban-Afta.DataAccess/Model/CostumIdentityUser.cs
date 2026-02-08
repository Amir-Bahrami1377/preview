namespace FormerUrban_Afta.DataAccess.Model
{
    public class CostumIdentityUser : IdentityUser
    {
        public string Name { get; set; }
        public string Family { get; set; }
        public string Hashed { get; set; }
        public override string ToString()
        {
            return $"{Id}{Name}{Family}{UserName}{NormalizedUserName}{Email?.Trim()}{NormalizedEmail?.Trim()}{EmailConfirmed}{PasswordHash}{PhoneNumber?.Trim()}" +
                   $"{PhoneNumberConfirmed}{TwoFactorEnabled}{LockoutEnd?.Ticks}{LockoutEnabled}{AccessFailedCount}{ConcurrencyStamp}{SecurityStamp}".Trim();
        }
    }
}
