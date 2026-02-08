using FormerUrban_Afta.DataAccess.Model.BaseEntity;

namespace FormerUrban_Afta.DataAccess.Model;
public class Erja : BaseModel
{
    public Nullable<int> sh_darkhast { get; set; }
    public string tarikh_darkhast { get; set; }
    public int c_nodarkhast { get; set; }
    public string noedarkhast { get; set; }
    public Nullable<int> c_marhaleh { get; set; }
    public string marhaleh { get; set; }
    public string code_nosazi { get; set; }
    public Nullable<double> shop { get; set; }
    public string name_mot { get; set; }
    public int tarikh_erja { get; set; }
    public string ijadkonandeh_c { get; set; }
    public string ijadkonandeh { get; set; }
    public string ersalkonandeh_c { get; set; }
    public string ersalkonandeh { get; set; }
    public string girandeh_c { get; set; }
    public string girandeh { get; set; }
    public Nullable<bool> flag { get; set; }
    public string saat_erja { get; set; }
    public Nullable<int> c_vaziatErja { get; set; }
    public string vaziatErja { get; set; }

    public override string ToString()
    {
        return string.Join("",
            sh_darkhast, tarikh_darkhast, c_nodarkhast, noedarkhast,
            CreateDateTime.Ticks, ModifiedDate?.Ticks ?? 0, c_marhaleh, marhaleh, code_nosazi,
            CreateUser, ModifiedUser, shop, name_mot, tarikh_erja, ijadkonandeh_c, ijadkonandeh
            , ersalkonandeh_c, ersalkonandeh, girandeh_c, girandeh, flag, saat_erja, c_vaziatErja, vaziatErja).Trim();
    }
}
