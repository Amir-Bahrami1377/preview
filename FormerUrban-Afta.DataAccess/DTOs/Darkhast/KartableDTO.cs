namespace FormerUrban_Afta.DataAccess.DTOs.Darkhast;

public class KartableDTO
{
    public long Identity { get; set; }
    public Nullable<double> shop { get; set; }
    public Nullable<bool> flag { get; set; }
    public Nullable<int> c_marhaleh { get; set; }
    public string marhaleh { get; set; }
    public string girandeh_c { get; set; }
    public string girandeh { get; set; }
    public string name_mot { get; set; }
    public string saat_erja { get; set; }
    public string noedarkhast { get; set; }
    public string code_nosazi { get; set; }
    public Nullable<int> sh_darkhast { get; set; }
    public int tarikh_erja { get; set; }
    public string ersalkonandeh { get; set; }
    public string tarikh_darkhast { get; set; }
    public string ControllerName { get; set; }
    public bool IsValid { get; set; } = true;
}
