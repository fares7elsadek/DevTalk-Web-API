using DevTalk.Domain.Entites;
using DevTalk.Domain.Repositories;
using DevTalk.Infrastructure.Data;
using Microsoft.Extensions.Caching.Distributed;

namespace DevTalk.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _db;
    private readonly IDistributedCache _cache;

    public IPostRepository Post { get; private set; }

    public ICommentRepository Comment { get; private set; }

    public ICommentVotesRepository CommentVotes { get; private set; }

    public IPostMediaRepository PostMedia { get; private set; }

    public IPostVotesRepository PostVotes { get; private set; }

    public IUserRepository User { get; private set; }

    public UnitOfWork(AppDbContext db,IDistributedCache cache)
    {
        _db = db;
        _cache = cache;
        //Post = new CachedRepository<Post>(new PostRepository(_db),
        //    _cache,"Post",_db);
        Post = new PostRepository(_db);
        Comment = new CommendRepository(_db);
        CommentVotes = new CommentVotesRepository(_db);
        PostMedia = new PostMediaRepository(_db);
        User = new UserRepository(_db);
        PostVotes = new PostVotesRepository(_db);
    }
    public async Task SaveAsync()
    {
        await _db.SaveChangesAsync();
    }
}
