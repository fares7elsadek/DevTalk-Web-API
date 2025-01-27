using DevTalk.Domain.Entites;

namespace DevTalk.Domain.Repositories;

public interface IPostVotesRepository:IRepositories<PostVotes>
{
    void Update(PostVotes entity);
}
