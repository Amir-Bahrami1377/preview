namespace FormerUrban_Afta.DataAccess.Services.Interfaces
{
    public interface IDv_malekinService
    {
        public Task<Dv_malekinDTO> GetById(long id);
        public Task<List<Dv_malekinDTO>> GetMalekinForParvande(int shop, long shod);
        public bool DeleteMalek(long identity, int shop);
        public Task<bool> InsertByModel(Dv_malekinDTO malek, string mtablename);
        public Task<bool> Update(Dv_malekinDTO malekin);
    }
}
