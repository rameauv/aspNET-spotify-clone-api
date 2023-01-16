using Microsoft.Extensions.Configuration;
using Npgsql;

namespace SystemTests.Fixtures;

public class DatabaseFixture : IDisposable
{
    private bool _isDisposed;

    public DatabaseFixture()
    {
        var config = InitConfiguration();
        var connectionString = config.GetConnectionString("DBContext");
        Db = new NpgsqlConnection(connectionString);
        Db.Open();
        using var cmd = new NpgsqlCommand("TRUNCATE \"Likes\", \"RefreshTokens\", \"Users\";", Db);
        cmd.ExecuteNonQuery();
    }

    ~DatabaseFixture()
    {
        Dispose(false);
    }

    public NpgsqlConnection Db { get; }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool isDisposing)
    {
        if (_isDisposed)
        {
            return;
        }

        if (isDisposing)
        {
            Db.Dispose();
        }

        _isDisposed = true;
    }

    public static IConfiguration InitConfiguration()
    {
        var env = new BuildEnvProvider().Env;
        var config = new ConfigurationBuilder()
            .AddJsonFile($"appsettings.{env}.json")
            .AddEnvironmentVariables()
            .Build();
        return config;
    }
}