namespace FormerUrban_Afta.DataAccess.Services.Interfaces;
public interface IBlockedIPRange
{
    public Task<List<BlockedIPRangeDto>?> GetAllBlockedIPRangeAsync();
    public Task<BlockedIPRangeDto?> GetBlockedIPRangeAsync(long id);
    public Task<bool> AddBlockedIPRangeAsync(BlockedIPRangeDto obj);
    public Task<bool> UpdateBlockedIPRangeAsync(BlockedIPRangeDto obj);
    public Task<bool> DeleteBlockedIPRangeAsync(long identity);
    public Task<BlockedIPRangeDto> GetBlockedIPByIdAsNoTracking(long id);
}

