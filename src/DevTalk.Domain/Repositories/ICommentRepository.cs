using DevTalk.Domain.Entites;

namespace DevTalk.Domain.Repositories;

public interface ICommentRepository:IRepositories<Comment>
{
    void Update (Comment comment);
}
