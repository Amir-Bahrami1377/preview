namespace FormerUrban_Afta.DataAccess.Model.BaseEntity;
public sealed class DbConfig
{
    public DbConfig(string conn) => ConnectionString = conn;

    public string ConnectionString { get; }
}

