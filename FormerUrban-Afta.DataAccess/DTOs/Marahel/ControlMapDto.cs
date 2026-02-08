namespace FormerUrban_Afta.DataAccess.DTOs.Marahel;
public class ControlMapDto
{
    public long Identity { get; set; }
    public int shop { get; set; }
    public int sh_Darkhast { get; set; }
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
    public bool IsValid { get; set; } = true;
    public List<string>? message { get; set; }
    public int codeMarhaleh { get; set; }
}
