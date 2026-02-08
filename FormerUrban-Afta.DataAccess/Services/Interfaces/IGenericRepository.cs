namespace FormerUrban_Afta.DataAccess.Services.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetAsync(long id);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<bool> ExistAsync(long id);
        Task<T> AddAsync(T entity);
        Task<List<T>> AddListAsync(List<T> entity);
        Task<T> UpdateAsync(T entity);
        Task<T> DeleteAsync(T entity);
        public T DecryptInfo(T entity);
        public T EncryptInfo(T entity);

    }
}
