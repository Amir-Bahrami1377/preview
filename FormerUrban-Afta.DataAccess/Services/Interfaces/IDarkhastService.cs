namespace FormerUrban_Afta.DataAccess.Services.Interfaces
{
    public interface IDarkhastService
    {
        public Task<bool> AddDarkhast(DarkhastDTO darkhast);
        public bool IsExistRequestNumber(int shDarkhast);
        public int GetLastShod();
        public Task<DarkhastDTO> GetDataByShod(int shod);
    }
}
