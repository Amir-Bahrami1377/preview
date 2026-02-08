namespace FormerUrban_Afta.DataAccess.DTOs.Marahel;

public class ParvanehDto
{
    public long Identity { get; set; }
    public int sh_darkhast { get; set; }
    public double sho_parvaneh { get; set; }
    public Nullable<int> c_noeParvaneh { get; set; }
    public string? noe_parvaneh { get; set; }
    public string tarikh_parvaneh { get; set; }
    public Nullable<double> masahat_m_s_tarakom { get; set; }
    public Nullable<double> masahat_m_esh_zamin { get; set; }
    public string? tarikh_end_amaliat_s { get; set; }
    public int sho_bimenameh { get; set; }
    public string tarikh_e_bimeh { get; set; }
    public string? tozihat_parvaneh { get; set; }
    public bool IsValid { get; set; } = true;
    public List<string>? message { get; set; }
    public int shop { get; set; }
    public int codeMarhaleh { get; set; }
}
