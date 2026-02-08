namespace FormerUrban_Afta.DataAccess.Services.Interfaces
{
    public interface IApartmanService
    {
        public bool updateActiveLastRow(int Shop);
        public Task UpdateByModel(ApartmanDto apartman);
        public void Copy_Apar(int Shop, long ShodG, long ShodJ);
        public void CopyUpdate_Apartman(int shop, long shodG, long shodJ);
        public Task<ApartmanDto> GetRowByShop(int shop);
        public Task<ApartmanDto> GetRowByShop(int shop, int shod);
        public Task InsertByModel(ApartmanDto obj);
    }
}
