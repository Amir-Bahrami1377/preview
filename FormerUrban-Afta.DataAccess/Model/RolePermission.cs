using FormerUrban_Afta.DataAccess.Model.BaseEntity;

namespace FormerUrban_Afta.DataAccess.Model;
public class RolePermission : BaseModel
{
    public string RoleId { get; set; } // Foreign key to IdentityRole
    public int PermissionId { get; set; } // Foreign key to Permission
    public CostumIdentityRole Role { get; set; }

    public override string ToString()
    {
        return string.Join("",
            RoleId, PermissionId, Role,
            CreateDateTime.Ticks, ModifiedDate?.Ticks ?? 0,
            CreateUser, ModifiedUser).Trim();
    }
}