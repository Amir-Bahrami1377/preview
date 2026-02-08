namespace FormerUrban_Afta.DataAccess.DTOs.IdentityUser
{
    public class UserPermissionDto
    {
        public long Identity { get; set; }
        public string UserId { get; set; } // Foreign key to IdentityUser
        public long PermissionId { get; set; } // Foreign key to Permission
        public string Permission_Name { get; set; } // e.g., "EditButton", "DeleteButton"
        public string Permission_Description { get; set; }
        public string CostumIdentityUser_Name { get; set; }
        public string CostumIdentityUser_Family { get; set; }
        public bool Access { get; set; }
        public string UserName { get; set; }
        public bool IsValid { get; set; } = true;
    }
}
