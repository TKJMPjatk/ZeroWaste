using System.Data;
namespace ZeroWaste.Data.DapperConnection;

public interface IDbConnectionFactory
{
    IDbConnection GetDbConnection();
}