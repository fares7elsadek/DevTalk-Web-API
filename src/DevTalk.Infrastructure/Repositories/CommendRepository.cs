using DevTalk.Domain.Entites;
using DevTalk.Domain.Repositories;
using DevTalk.Infrastructure.Data;

namespace DevTalk.Infrastructure.Repositories;

public class CommendRepository(AppDbContext _db) : Repository<Comment>(_db), ICommentRepository
{
    public void Update(Comment comment)
    {
        _db.Update(comment);
    }
}
