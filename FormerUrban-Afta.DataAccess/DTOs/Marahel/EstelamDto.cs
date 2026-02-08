namespace FormerUrban_Afta.DataAccess.DTOs.Marahel;
public class EstelamDto
{
    public long Identity { get; set; }
    public int Sh_Darkhast { get; set; }
    public int shop { get; set; }
    public string Sh_Pasokh { get; set; }
    public string Tarikh_Pasokh { get; set; }
    public double Dang_Enteghal { get; set; }
    public string Kharidar { get; set; }
    public string Tozihat { get; set; }
    public long codeNoeMalekiat { get; set; }
    public string NoeMalekiat { get; set; }
    public int codeMarhaleh { get; set; }
    public List<string>? message { get; set; }
    public bool IsValid { get; set; } = true;

}