using Microsoft.Extensions.Configuration;
using Npgsql;

namespace SMTG.API.Services;

public class DatabaseService
{
    private readonly IConfiguration _configuration;

    public DatabaseService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public NpgsqlConnection GetConnection()
    {
        var connectionString = new NpgsqlConnectionStringBuilder
        {
            Host = _configuration["Database:Host"],
            Port = int.Parse(_configuration["Database:Port"]!),
            Database = _configuration["Database:Name"],
            Username = _configuration["Database:User"],
            Password = _configuration["Database:Password"],
            SslMode = SslMode.Require
        }.ConnectionString;

        return new NpgsqlConnection(connectionString);
    }
}