using DevTalk.Domain.Entites;
using DevTalk.Domain.Repositories;
using DevTalk.Infrastructure.Data;

namespace DevTalk.Infrastructure.Repositories;

public class CommentVotesRepository(AppDbContext _db) : Repository<CommentVotes>(_db), ICommentVotesRepository
{
    public void Update(CommentVotes entity)
    {
        _db.Update(entity);
    }
}
