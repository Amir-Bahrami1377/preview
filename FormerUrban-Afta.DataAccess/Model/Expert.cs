using FormerUrban_Afta.DataAccess.Model.BaseEntity;

namespace FormerUrban_Afta.DataAccess.Model;
public class Expert : BaseModel
{
    public string Name { get; set; }
    public string Family { get; set; }
    public int RequestNumber { get; set; }
    public DateTime DateVisit { get; set; }

    public override string ToString()
    {
        return string.Join("", CreateDateTime.Ticks, ModifiedDate?.Ticks ?? 0, CreateUser, ModifiedUser, Name,
            Family, RequestNumber, DateVisit).Trim();
    }
}