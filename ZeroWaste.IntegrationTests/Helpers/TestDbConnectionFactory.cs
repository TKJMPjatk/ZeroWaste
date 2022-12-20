using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ZeroWaste.Data.DapperConnection;

namespace ZeroWaste.IntegrationTests.Helpers;

public class TestDbConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;
    private IDbConnection _connection;
    public TestDbConnectionFactory(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("TestConnectionString");
    }
    public IDbConnection GetDbConnection()
    {
        //if (_connection == null && string.IsNullOrEmpty(_connection.ConnectionString))
        _connection = new SqlConnection(_connectionString);
        return _connection;
    }
}