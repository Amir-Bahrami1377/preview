using FormerUrban_Afta.DataAccess.Model.BaseEntity;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace FormerUrban_Afta.DataAccess.Services;

public class SqlService : ISqlService
{
    IConfiguration _configuration;
    private readonly string _conn;

    public SqlService(IConfiguration configuration, DbConfig cfg)
    {
        _configuration = configuration;
        _conn = cfg.ConnectionString;
    }

    public string GetConnectionString() => _configuration.GetConnectionString("DefaultConnection") ?? "";

    private Semaphore Semaphore { get; set; } = new(1, 1);

    public UsedSpaceDto GetUsedSpaceByTableName(string tableName)
    {
        var connectionString = GetConnectionString();
        using var connection = new SqlConnection(connectionString);
        using var command = new SqlCommand("sp_spaceused", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.AddWithValue("@objname", tableName);
        connection.Open();

        var usedSpace = new UsedSpaceDto();
        var reader = command.ExecuteReader();
        if (reader.HasRows)
        {
            try
            {
                while (reader.Read())
                {
                    usedSpace.Name = (string)reader["name"];
                    usedSpace.Rows = int.Parse((string)reader["rows"]);
                    usedSpace.DataKB = int.Parse(((string)reader["data"]).ToLower().Replace(" kb", string.Empty));
                    usedSpace.UnusedKB = int.Parse(((string)reader["unused"]).ToLower().Replace(" kb", string.Empty));
                    usedSpace.ReservedKB = int.Parse(((string)reader["reserved"]).ToLower().Replace(" kb", string.Empty));
                    usedSpace.IndexSizeKB = int.Parse(((string)reader["index_size"]).ToLower().Replace(" kb", string.Empty));
                }
            }

            catch (Exception ex)
            {
            }
        }

        connection.Close();
        return usedSpace;
    }


    public long GetDriveAvailableFreeSpace(string driveName, DiskSpaceUnit unit = DiskSpaceUnit.Byte) => DriveInfo.GetDrives().FirstOrDefault(x =>
        x.IsReady && string.Equals(x.Name.ToLower(), driveName.ToLower(), StringComparison.Ordinal))?.AvailableFreeSpace / (int)unit ?? -1;
}

public enum DiskSpaceUnit
{
    Byte = 1,
    Kilobyte = 1024,
    Megabyte = 1024 * 1024,
    Gigabyte = 1024 * 1024 * 1024,
}

public class UsedSpaceDto
{
    public int Rows { get; set; }
    public int DataKB { get; set; }
    public string Name { get; set; }
    public int UnusedKB { get; set; }
    public int ReservedKB { get; set; }
    public int IndexSizeKB { get; set; }
    public double DataMB { get => DataKB / 1024.0; }
}
