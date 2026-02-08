namespace FormerUrban_Afta.DataAccess.Services
{
    public class WeakPasswordService : IWeakPasswordService
    {
        private readonly FromUrbanDbContext _context;

        public WeakPasswordService(FromUrbanDbContext context)
        {
            _context = context;
        }
        public List<WeakPassword> GetAllData() => _context.WeakPassword.ToList();

        public WeakPassword? Search(string password) => _context.WeakPassword.FirstOrDefault(a => a.WeakPasswordText.Contains(password));
    }
}
