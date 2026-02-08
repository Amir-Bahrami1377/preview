using FormerUrban_Afta.DataAccess.Model.BaseEntity;

namespace FormerUrban_Afta.DataAccess.Model;

public class Parvaneh : BaseModel
{
    public int shop { get; set; }
    public int sh_darkhast { get; set; }
    public double sho_parvaneh { get; set; }
    public Nullable<int> c_noeParvaneh { get; set; }
    public string? noe_parvaneh { get; set; }
    public DateTime? tarikh_parvaneh { get; set; }
    public Nullable<double> masahat_m_s_tarakom { get; set; }
    public Nullable<double> masahat_m_esh_zamin { get; set; }
    public string? tarikh_end_amaliat_s { get; set; }
    public int sho_bimenameh { get; set; }
    public DateTime? tarikh_e_bimeh { get; set; }
    public string? tozihat_parvaneh { get; set; }
    public override string ToString()
    {
        return $"{shop}{sh_darkhast}{sho_parvaneh}{c_noeParvaneh}{noe_parvaneh}{tarikh_parvaneh}{masahat_m_esh_zamin}{masahat_m_s_tarakom}" +
            $"{tarikh_end_amaliat_s}{sho_bimenameh}{tarikh_e_bimeh}{tozihat_parvaneh}" +
            $"{CreateDateTime.Ticks}{ModifiedDate?.Ticks ?? 0}{CreateUser}{ModifiedUser}".Trim();
    }
}
