using DevTalk.Domain.Entites;

namespace DevTalk.Domain.Repositories;

public interface IUnitOfWork
{
    IPostRepository Post { get; }
    ICommentRepository Comment { get; }
    ICommentVotesRepository CommentVotes { get; }
    IPostMediaRepository PostMedia { get; }
    IPostVotesRepository PostVotes { get; }
    IUserRepository User { get; }
    Task SaveAsync();
}
