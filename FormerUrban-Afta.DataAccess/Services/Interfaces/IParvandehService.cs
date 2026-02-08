namespace FormerUrban_Afta.DataAccess.Services.Interfaces
{
    public interface IParvandehService
    {
        public ParvandehDto GetRow(string shop);
        public ParvandehDto GetRowByCodeN(string CodeN);
        public int GetParvandehType(string shop);
        public bool CheckExistSenfi(decimal mantagheh, decimal hozeh, decimal blok, decimal shomelk, decimal sakhteman, decimal apar);
        public bool CheckExistApartman(decimal mantagheh, decimal hozeh, decimal blok, decimal shomelk, decimal sakhteman);
        public bool CheckSakhtemanHasSenf(decimal mantagheh, decimal hozeh, decimal blok, decimal shomelk, decimal sakhteman);
        public ParvandehDto GetRow(long mantaghe, long hoze, long blok, long melk, long sakh, long apar, long senfi);
        public void InsertByModel(ParvandehDto obj);
        public void UpdateByModel(ParvandehDto obj);
        public int CheckCountParvandeh(int mantagheh, int hozeh, int blok, int melk, int sakhteman, int aparteman, int senf);
        public int CheckCountParvandehByCodeTree(int mantagheh, int hozeh, int blok, int melk, double codeTree);
        public int GetMaxShop();
        public List<ParvandehDto> GetByCodeNAndCodeTree(string codeN, int codeTree);
        public ParvandehDto GetRowForTreeViewByCodeN(string CodeN);
        public long GetShodMojud(int shop);
        public bool copyForSabtDarkhast(int Shop, long ShodG, long ShodJ);
        public int GetAreaId(int shop);
    }
}
