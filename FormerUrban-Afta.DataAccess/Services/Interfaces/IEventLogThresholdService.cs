namespace FormerUrban_Afta.DataAccess.Services.Interfaces
{
    public interface IEventLogThresholdService
    {
        public Task<EventLogTableUsedSpaceLevel> DoCheck(EventLogTableType tableType);
        public Task<EventLogThresholdDto> GetAsync();
        public Task<EventLogThresholdDto> Get();
        public Task<EventLogThresholdDto> GetAsNoTracking();
        public Task<bool> Update(EventLogThresholdDto command);
        public Task<bool> AddAsync(EventLogThresholdDto obj);
        public Task<bool> ExistsAsync();
        public Task<EventLogThresholdDto> GetUserDrp(EventLogThresholdDto model);
    }
}
