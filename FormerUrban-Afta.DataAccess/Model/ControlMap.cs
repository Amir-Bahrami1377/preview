using FormerUrban_Afta.DataAccess.Model.BaseEntity;

namespace FormerUrban_Afta.DataAccess.Model;

public class ControlMap : BaseModel
{
    public int shop { get; set; }
    public int sh_Darkhast { get; set; }
    //مساحت طبق سند
    public double? masahat_s { get; set; }
    //مساحت موجود
    public double? masahat_m { get; set; }
    //مساحت اصلاحی
    public double? masahat_e { get; set; }
    //مساحت باقی مانده
    public double? masahat_b { get; set; }
    public Nullable<int> C_NoeNama { get; set; }
    public string NoeNama { get; set; }
    public Nullable<int> c_noesaghf { get; set; }
    public string noesaghf { get; set; }
    public Nullable<double> tarakom { get; set; }
    public Nullable<double> satheshghal { get; set; }
    public string NoeSaze { get; set; }
    public Nullable<int> c_NoeSaze { get; set; }
    public Nullable<double> TedadTabaghe { get; set; }
    public override string ToString()
    {
        return string.Join("",
            shop, sh_Darkhast, masahat_s ?? 0, masahat_m ?? 0, masahat_e ?? 0,
            masahat_b ?? 0, C_NoeNama ?? 0, NoeNama, c_noesaghf ?? 0, noesaghf,
            tarakom ?? 0, satheshghal ?? 0, NoeSaze, c_NoeSaze ?? 0,
            TedadTabaghe ?? 0, CreateDateTime.Ticks, ModifiedDate?.Ticks ?? 0,
            CreateUser, ModifiedUser).Trim();
    }
}
