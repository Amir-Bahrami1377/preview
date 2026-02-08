namespace FormerUrban_Afta.DataAccess.Services.Interfaces
{
    public interface IWeakPasswordService
    {
        List<WeakPassword> GetAllData();
        WeakPassword? Search(string password);
    }
}
