namespace FormerUrban_Afta.DataAccess.Services.Interfaces
{
    public interface ISakhtemanService
    {
        public decimal Insert(int shop, string marhale, int c_marhale, string tozihat);
        public bool UpdateActiveLastRow(int Shop);
        public bool Update(int shop, decimal radif, string marhale, int c_marhale, string tozihat);
        public void UpdateByModel(SakhtemanDto obj);
        public void Copy_Sakhteman(int Shop, long ShodG, long ShodJ);
        public void CopyUpdate_Sakhteman(int shop, long shodG, long shodJ);
        public SakhtemanDto GetDataByShop(int Shop);
        public SakhtemanDto GetDataByShop(int Shop, int Sh_Darkhast);
        public void InsetByModel(SakhtemanDto obj);

    }
}
