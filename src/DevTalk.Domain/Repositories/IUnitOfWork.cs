using DevTalk.Domain.Entites;
using System.Data;

namespace DevTalk.Domain.Repositories;

public interface IUnitOfWork
{
    IPostRepository Post { get; }
    ICommentRepository Comment { get; }
    ICommentVotesRepository CommentVotes { get; }
    IPostMediaRepository PostMedia { get; }
    IPostVotesRepository PostVotes { get; }
    IUserRepository User { get; }
    ICategoryRepository Category { get; }
    IBookmarkRepository Bookmark { get; }
    IPreferenceRepository Preference { get; }
    INotificationRepostiory Notification { get; }
    Task SaveAsync();
    public IDbTransaction BeginTransaction();
}
