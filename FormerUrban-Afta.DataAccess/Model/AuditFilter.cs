using FormerUrban_Afta.DataAccess.Model.BaseEntity;

namespace FormerUrban_Afta.DataAccess.Model;

public class AuditFilter : BaseModel
{
    public EnumFormName FormId { get; set; }

    public override string ToString() => string.Join("", FormId, CreateDateTime.Ticks, ModifiedDate?.Ticks ?? 0, CreateUser, ModifiedUser).Trim();

}
