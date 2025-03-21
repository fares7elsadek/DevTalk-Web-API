using DevTalk.Domain.Entites;
using System.Linq.Expressions;

namespace DevTalk.Domain.Repositories;

public interface IPostRepository:IRepositories<Post>
{
    Task<IEnumerable<Post>> GetAllPostsPagination(string cursor, int pageSize);
    Task<IEnumerable<Post>> GetTrendingPostsPagination(string idCursor, DateTime? dateTimeCursor, double? scoreCursor, int pageSize);
    Task<IEnumerable<Post>> GetFeedPostsPagination(string idCursor,string userId, DateTime? dateTimeCursor, double? scoreCursor, int pageSize);
    
}
