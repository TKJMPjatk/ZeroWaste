using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ZeroWaste.Data.DapperConnection;

public class DbConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;
    private IDbConnection _connection;
    public DbConnectionFactory(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnectionString");
    }
    public IDbConnection GetDbConnection()
    {
        //if (_connection == null && string.IsNullOrEmpty(_connection.ConnectionString))
            _connection = new SqlConnection(_connectionString);
        return _connection;
    }
}