namespace Company.Framework.Data.Db.Settings;

public class DbProviderSettings
{
    public DbConnectionSettings Connection { get; init; }
    public DbContextSettings[] Contexts { get; init; }
}