using DevTalk.Domain.Entites;

namespace DevTalk.Domain.Repositories;

public interface IUserRepository:IRepositories<User>
{
    void Update(User user);
}
