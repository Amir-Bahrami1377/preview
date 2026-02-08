namespace FormerUrban_Afta.DataAccess.DTOs.IdentityUser
{
    public class PermissionDto
    {
        public int Id { get; set; }
        public string Name { get; set; } // e.g., "EditButton", "DeleteButton"
        public string Description { get; set; }
    }
}
