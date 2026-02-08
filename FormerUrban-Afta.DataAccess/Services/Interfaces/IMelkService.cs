namespace FormerUrban_Afta.DataAccess.Services.Interfaces
{
    public interface IMelkService
    {
        public Task<MelkDto> GetData(int shop, int shod);
        public Task<MelkDto> GetDataByShop(int shop);
        public Task<MelkDto> GetDataByRadif(int shop, int radif);
        public decimal GetRadif(int shop, long shod);
        public decimal GetRadif(int shop);
        public Task Update(MelkDto obj);
        public void Copy_Melk(int Shop, long ShodG, long ShodJ);
        public void CopyUpdate_Melk(int shop, long shodG, long shodJ);
        public Task InsetByModel(MelkDto obj);
    }
}
