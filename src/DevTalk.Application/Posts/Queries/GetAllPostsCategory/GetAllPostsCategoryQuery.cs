using DevTalk.Application.Caching;
using DevTalk.Application.Posts.Dtos;
using DevTalk.Domain.Entites;

namespace DevTalk.Application.Posts.Queries.GetAllPostsCategory;

public class GetAllPostsCategoryQuery(
    string idCursor, string timeCursor
    , double scoreCursor, int pageSize,string categoryId) :ICachableRequest<GetUserPostsDto>
{
    public string IdCursor { get; set; } = idCursor;
    public string timeCursor { get; set; } = timeCursor;
    public double ScoreCursor { get; set; } = scoreCursor;
    public int PageSize { get; set; } = pageSize;
    public string CategoryId { get; set; } = categoryId;
    public string Key => $"post:category:{CategoryId}:{IdCursor}:{timeCursor}:{PageSize}";
    public TimeSpan? CacheExpiryTime => throw new NotImplementedException();
}
