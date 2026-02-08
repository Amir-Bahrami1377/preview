using FormerUrban_Afta.DataAccess.Model.Enums;

namespace FormerUrban_Afta.DataAccess.Services.Interfaces;
public interface IErjaService
{
    public double GetDataWithShopNoeDarkhastVaziatErja(int shop, int c_noeDarkhast, int c_vaziatErja);
    public bool InsertForSabtDarkhast(DarkhastDTO darkhast);
    public List<KartableDTO> GetKartable();
    Task<List<KartableDTO>> GetBaygani();
    public List<Erja> GetActiveData(int shod);
    public bool UpdateData(List<Erja> erjas);
    public bool InsertForNextMarhaleh(DarkhastDTO darkhast, EnumMarhalehTypeInfo nextMarhale);
}