namespace Company.Framework.Data.Settings;

public class DbInstanceSettings
{
    public DbType Type { get; init; }

    public string Name { get; init; }
    public DbProviderSettings Provider { get; set; }
}