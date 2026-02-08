namespace FormerUrban_Afta.DataAccess.DTOs.IdentityUser
{
    public class RolePermissionDto
    {
        public long Identity { get; set; }
        public string RoleId { get; set; }
        public long PermissionId { get; set; }
        public bool Access { get; set; }
        public string PermissionName { get; set; }
        public string roleName { get; set; }
        public bool IsValid { get; set; } = true;
    }
}
