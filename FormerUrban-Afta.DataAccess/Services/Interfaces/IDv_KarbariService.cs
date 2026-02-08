using FormerUrban_Afta.DataAccess.DTOs.Parvandeh;

namespace FormerUrban_Afta.DataAccess.Services.Interfaces
{
    public interface IDv_KarbariService
    {
        public List<Dv_karbariDTO> GetDataByRadif(int shop, decimal radif);
        public bool UpdateKarbari(Dv_karbariDTO dv_KarbariDTO);
        public bool DeleteByModel(Dv_karbariDTO karbari);
        public bool InsertByModel(Dv_karbariDTO karbari, string mtableName);
        public Dv_karbariDTO GetById(long id);  
        public bool DeleteById(long identity, int shop);


    }
}
