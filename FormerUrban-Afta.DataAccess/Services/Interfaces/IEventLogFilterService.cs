namespace FormerUrban_Afta.DataAccess.Services.Interfaces
{
    public interface IEventLogFilterService
    {
        public EventLogFilterDto Get();
        public EventLogFilterDto GetAsNoTracking();

        public Task<bool> Add(EventLogFilterDto command);
        public Task<bool> Update(EventLogFilterDto command);
        public bool CheckHash(EventLogFilter obj);
        public Task<bool> Exists();
    }
}
