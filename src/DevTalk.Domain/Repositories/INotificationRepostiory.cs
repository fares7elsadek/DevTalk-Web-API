using DevTalk.Domain.Entites;

namespace DevTalk.Domain.Repositories;

public interface INotificationRepostiory:IRepositories<Notifications>
{
    public Task AddRange(List<Notifications> list);
}
