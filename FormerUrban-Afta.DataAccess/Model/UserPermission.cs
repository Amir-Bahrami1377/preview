using FormerUrban_Afta.DataAccess.Model.BaseEntity;

namespace FormerUrban_Afta.DataAccess.Model
{
    public class UserPermission : BaseModel
    {
        public string UserId { get; set; } // Foreign key to IdentityUser
        public int PermissionId { get; set; } // Foreign key to Permission
        public CostumIdentityUser User { get; set; }

        public override string ToString()
        {
            return string.Join("",
                UserId, PermissionId, User.ToString(),
                CreateDateTime.Ticks, ModifiedDate?.Ticks ?? 0,
                CreateUser, ModifiedUser).Trim();
        }
    }

}
