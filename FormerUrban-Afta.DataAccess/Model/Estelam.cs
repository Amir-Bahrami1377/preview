using FormerUrban_Afta.DataAccess.Model.BaseEntity;

namespace FormerUrban_Afta.DataAccess.Model;
public class Estelam : BaseModel
{
    public int Sh_Darkhast { get; set; }
    public string Sh_Pasokh { get; set; }
    public DateTime? Tarikh_Pasokh { get; set; }
    public double Dang_Enteghal { get; set; }
    public string Kharidar { get; set; }
    public string Tozihat { get; set; }
    public long codeNoeMalekiat { get; set; }
    public string NoeMalekiat { get; set; }

    public override string ToString()
    {
        return string.Join("",
            Sh_Darkhast, Sh_Pasokh, Tarikh_Pasokh?.Ticks ?? 0, Dang_Enteghal,
            Kharidar, Tozihat, codeNoeMalekiat, NoeMalekiat,
            CreateDateTime.Ticks, ModifiedDate?.Ticks ?? 0,
            CreateUser, ModifiedUser).Trim();
    }
}