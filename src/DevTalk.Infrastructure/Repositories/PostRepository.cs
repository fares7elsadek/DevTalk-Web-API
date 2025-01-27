using DevTalk.Domain.Entites;
using DevTalk.Domain.Repositories;
using DevTalk.Infrastructure.Data;

namespace DevTalk.Infrastructure.Repositories;

public class PostRepository(AppDbContext _db) : Repository<Post>(_db), IPostRepository
{
    public void Update(Post post)
    {
        _db.Update(post);
    }
}
