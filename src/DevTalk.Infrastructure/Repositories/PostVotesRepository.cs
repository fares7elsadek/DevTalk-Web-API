using DevTalk.Domain.Entites;
using DevTalk.Domain.Repositories;
using DevTalk.Infrastructure.Data;

namespace DevTalk.Infrastructure.Repositories;

public class PostVotesRepository(AppDbContext _db) : Repository<PostVotes>(_db), IPostVotesRepository
{
    
}
