using System.Data;
using Dapper;
using DevTalk.Domain.Abstractions;
using DevTalk.Domain.Entites;
using DevTalk.Domain.Repositories;
using DevTalk.Infrastructure.Data;

namespace DevTalk.Infrastructure.Repositories;

public class NotificationRepository(AppDbContext db,ISqlConnectionFactory dapper) : Repository<Notifications>(db), INotificationRepostiory
{
    public async Task AddRange(List<Notifications> list)
    {
        await db.Notifications.AddRangeAsync(list);
    }

    public async Task<IEnumerable<Notifications>> GetUserNotifications(string userId, DateTime? cursor,int pageSize)
    {
        var sql= @" -- get notifications for user
                    SELECT * FROM ""Notifications""
                    WHERE ""UserId"" = @userId
                    AND (@cursor::timestamp IS NULL OR ""Timestamp"" < @cursor::timestamp)
                    ORDER BY ""Timestamp"" DESC
                    LIMIT @pageSize; ";
        
        using IDbConnection connection = dapper.CreateConnection();
        var notifications = await connection.QueryAsync<Notifications>(sql, new { userId,cursor,pageSize });
        return notifications;
    }
}
