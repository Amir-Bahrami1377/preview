using FormerUrban_Afta.DataAccess.Model.BaseEntity;

namespace FormerUrban_Afta.DataAccess.Model;

public class ActivityLogFilters : BaseModel
{
    public EnumFormName FormName { get; set; }
    public bool AddStatus { get; set; } = true;
    public bool UpdateStatus { get; set; } = true;
    public bool GetStatus { get; set; } = true;
    public bool DeleteStatus { get; set; } = true;

    public override string ToString()
    {
        return string.Join("",
                FormName, AddStatus, UpdateStatus, GetStatus, DeleteStatus,
                CreateDateTime.Ticks, ModifiedDate?.Ticks ?? 0,
                CreateUser, ModifiedUser).Trim();
    }
}