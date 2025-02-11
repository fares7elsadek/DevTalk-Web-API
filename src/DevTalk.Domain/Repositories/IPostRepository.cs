using DevTalk.Domain.Entites;
using System.Linq.Expressions;

namespace DevTalk.Domain.Repositories;

public interface IPostRepository:IRepositories<Post>
{
    Task<IEnumerable<Post>> GetAllPostsPagination(string cursor, int pageSize, string? IncludeProperties = null);
    Task<IEnumerable<Post>> GetTrendingPostsPagination(string idCursor, DateTime? dateTimeCursor, double? scoreCursor, int pageSize, string? IncludeProperties = null);
    Task<IEnumerable<Post>> GetFeedPostsPagination(string idCursor,List<string> prefernceCategories, DateTime? dateTimeCursor, double? scoreCursor, int pageSize, string? IncludeProperties = null);
}
