using FormerUrban_Afta.DataAccess.Model.BaseEntity;

namespace FormerUrban_Afta.DataAccess.Model;

public class Dv_savabegh : BaseModel
{
    public int shop { get; set; }
    public int d_radif { get; set; }
    public string mtable_name { get; set; }
    public Nullable<int> c_noe { get; set; }
    public string noe { get; set; }
    public int sh_darkhast { get; set; }
    public override string ToString()
    {
        return $"{shop}{d_radif}{mtable_name?.Trim()}{c_noe ?? 0}{noe?.Trim()}{sh_darkhast}{CreateDateTime.Ticks}{ModifiedDate?.Ticks ?? 0}{CreateUser}{ModifiedUser}";
    }
}
