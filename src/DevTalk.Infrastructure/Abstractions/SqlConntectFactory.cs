using System.Data;
using DevTalk.Domain.Abstractions;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace DevTalk.Infrastructure.Abstractions;

public class SqlConntectFactory(IConfiguration configuration):ISqlConnectionFactory
{
    public IDbConnection CreateConnection()
    {
        return new NpgsqlConnection(configuration.GetConnectionString("cs"));
    }
}