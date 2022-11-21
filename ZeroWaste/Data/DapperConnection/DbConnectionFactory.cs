using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ZeroWaste.Data.DapperConnection;

public class DbConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;
    private IDbConnection _connection;
    public DbConnectionFactory(AppDbContext appDbContext)
    {
        _connectionString = "Server=tcp:zero-waste.database.windows.net,1433;Initial Catalog=zeroWaste;Persist Security Info=False;User ID=zerowaste-admin;Password=RUCH200nowe;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
    }
    public IDbConnection GetDbConnection()
    {
        if (_connection == null)
            _connection = new SqlConnection(_connectionString);
        return _connection;
    }
}