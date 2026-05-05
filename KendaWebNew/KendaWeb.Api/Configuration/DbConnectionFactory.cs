using System.Data;
using Microsoft.Data.SqlClient;

namespace KendaWeb.Api.Configuration;

public interface IDbConnectionFactory
{
    IDbConnection CreateErpConnection();
    IDbConnection CreateBbConnection();
    IDbConnection CreateMfnsConnection(string ip);
    IDbConnection CreateCwssConnection(string ip);
}

public class DbConnectionFactory : IDbConnectionFactory
{
    private readonly IConfiguration _config;

    public DbConnectionFactory(IConfiguration config)
    {
        _config = config;
    }

    public IDbConnection CreateErpConnection()
    {
        return new SqlConnection(_config.GetConnectionString("ErpHome"));
    }

    public IDbConnection CreateBbConnection()
    {
        return new SqlConnection(_config.GetConnectionString("BbHome"));
    }

    public IDbConnection CreateMfnsConnection(string ip)
    {
        var template = _config["MachineConfig:MfnsDbTemplate"] ?? "";
        return new SqlConnection(template.Replace("{ip}", ip));
    }

    public IDbConnection CreateCwssConnection(string ip)
    {
        var template = _config["MachineConfig:CwssDbTemplate"] ?? "";
        return new SqlConnection(template.Replace("{ip}", ip));
    }
}
