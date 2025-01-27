using DevTalk.Domain.Entites;
using DevTalk.Domain.Repositories;
using DevTalk.Infrastructure.Data;

namespace DevTalk.Infrastructure.Repositories;

public class PostMediaRepository(AppDbContext _db) : Repository<PostMedia>(_db), IPostMediaRepository
{
    public void Update(PostMedia postMedia)
    {
        _db.Update(postMedia);
    }
}
