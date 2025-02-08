using DevTalk.Domain.Entites;
using DevTalk.Domain.Repositories;
using DevTalk.Infrastructure.Data;

namespace DevTalk.Infrastructure.Repositories;

public class CommentRepository(AppDbContext _db) : Repository<Comment>(_db), ICommentRepository
{
    
}
