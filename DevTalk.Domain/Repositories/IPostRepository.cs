using DevTalk.Domain.Entites;

namespace DevTalk.Domain.Repositories;

public interface IPostRepository:IRepositories<Post>
{
    public void Update(Post post);  
}
