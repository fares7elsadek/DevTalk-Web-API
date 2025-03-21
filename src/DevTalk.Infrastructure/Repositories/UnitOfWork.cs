using DevTalk.Domain.Entites;
using DevTalk.Domain.Repositories;
using DevTalk.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Caching.Distributed;
using System.Data;
using DevTalk.Domain.Abstractions;

namespace DevTalk.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _db;
    private readonly ISqlConnectionFactory _connectionFactory;

    public IPostRepository Post { get; private set; }
    public ICommentRepository Comment { get; private set; }
    public ICommentVotesRepository CommentVotes { get; private set; }
    public IPostMediaRepository PostMedia { get; private set; }
    public IPostVotesRepository PostVotes { get; private set; }
    public IUserRepository User { get; private set; }
    public ICategoryRepository Category { get; private set; }
    public IBookmarkRepository Bookmark { get; private set; }
    public IPreferenceRepository Preference { get; private set; }
    public INotificationRepostiory Notification { get; private set; }

    public UnitOfWork(AppDbContext db,IDistributedCache cache,ISqlConnectionFactory connectionFactory)
    {
        _db = db;
        _connectionFactory = connectionFactory;
        Post = new PostRepository(_db,_connectionFactory);
        Comment = new CommentRepository(_db);
        CommentVotes = new CommentVotesRepository(_db);
        PostMedia = new PostMediaRepository(_db);
        User = new UserRepository(_db);
        PostVotes = new PostVotesRepository(_db);
        Category = new CategoryRepository(_db);
        Bookmark = new BookmarkRepository(_db);
        Preference = new PreferenceRepository(_db);
        Notification = new NotificationRepository(_db);
    }
    public async Task SaveAsync()
    {
        await _db.SaveChangesAsync();
    }

    public IDbTransaction BeginTransaction()
    {
        var transaction = _db.Database.BeginTransaction();
        return transaction.GetDbTransaction();
    }
}
