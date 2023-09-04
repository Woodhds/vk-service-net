using System.Data.Common;
using Microsoft.Data.Sqlite;

namespace VkService.Data;

public interface IDbConnectionFactory
{
    DbConnection GetConnection();
}

public class DbConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public DbConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public DbConnection GetConnection()
    {
        return new SqliteConnection(_connectionString);
    }
}
