namespace FormerUrban_Afta.DataAccess.Services.Interfaces;
public interface IAllowedIPRange
{
    public Task<List<AllowedIPRangeDto>?> GetAllAllowedIPRangeAsync();
    public Task<AllowedIPRangeDto?> GetAllowedIPRangeAsync(long id);
    public Task<bool> AddAllowedIPRangeAsync(AllowedIPRangeDto obj);
    public Task<bool> UpdateAllowedIPRangeAsync(AllowedIPRangeDto obj);
    public Task<bool> DeleteAllowedIPRangeAsync(long id);
    public Task<AllowedIPRangeDto> GetByIdAsNoTracking(long id);
}

