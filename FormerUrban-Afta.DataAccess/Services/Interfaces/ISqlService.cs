namespace FormerUrban_Afta.DataAccess.Services.Interfaces
{
    public interface ISqlService
    {
        public string GetConnectionString();
        public UsedSpaceDto GetUsedSpaceByTableName(string tableName);
        //public int EventLogBulkRemove(EventLogTableType tableType, int percentage);
        public long GetDriveAvailableFreeSpace(string driveName, DiskSpaceUnit unit = DiskSpaceUnit.Byte);
    }
}
