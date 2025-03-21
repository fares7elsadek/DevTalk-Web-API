using System.Data;

namespace DevTalk.Domain.Abstractions;

public interface ISqlConnectionFactory
{
   IDbConnection CreateConnection();
}