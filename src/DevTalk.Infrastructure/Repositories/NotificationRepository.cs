using DevTalk.Domain.Entites;
using DevTalk.Domain.Repositories;
using DevTalk.Infrastructure.Data;

namespace DevTalk.Infrastructure.Repositories;

public class NotificationRepository(AppDbContext db) : Repository<Notifications>(db), INotificationRepostiory
{
    public async Task AddRange(List<Notifications> list)
    {
        await db.Notifications.AddRangeAsync(list);
    }
}
