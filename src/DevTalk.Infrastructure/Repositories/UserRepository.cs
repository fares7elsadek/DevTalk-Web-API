using DevTalk.Domain.Entites;
using DevTalk.Domain.Repositories;
using DevTalk.Infrastructure.Data;

namespace DevTalk.Infrastructure.Repositories;

public class UserRepository(AppDbContext _db) : Repository<User>(_db), IUserRepository
{
    public void Update(User user)
    {
        _db.Update(user); 
    }
}
