using FormerUrban_Afta.DataAccess.Model.BaseEntity;

namespace FormerUrban_Afta.DataAccess.Model;

public sealed class AllowedIPRange : BaseModel
{
    public string IPRange { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public override string ToString()
    {
        return string.Join("",
            IPRange, Description, FromDate?.Ticks ?? 0, ToDate?.Ticks ?? 0,
            CreateDateTime.Ticks, ModifiedDate?.Ticks ?? 0,
            CreateUser, ModifiedUser).Trim();
    }
}

