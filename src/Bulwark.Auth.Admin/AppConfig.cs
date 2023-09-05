namespace Bulwark.Auth.Admin;

public class AppConfig
{
    public string DbConnection { get; }
    public string DbNameSeed { get; }
    public AppConfig()
    {
        DbConnection = Environment.GetEnvironmentVariable("DB_CONNECTION") ?? "mongodb://localhost:27017";
        DbNameSeed = Environment.GetEnvironmentVariable("DB_NAME_SEED") ?? string.Empty;
    }
}