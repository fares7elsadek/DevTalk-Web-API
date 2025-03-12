using DevTalk.Application.Caching;
using DevTalk.Application.Posts.Dtos;
using MediatR;

namespace DevTalk.Application.Posts.Queries.GetTrendingPosts;

public class GetTrendingPostsQuery(string idCursor,string timeCursor
    ,double scoreCursor,int pageSize):ICachableRequest<GetUserPostsDto>
{
    public string IdCursor { get; set; } = idCursor;
    public string timeCursor { get; set; } = timeCursor;
    public double ScoreCursor { get; set; } = scoreCursor;
    public int PageSize { get; set; } = pageSize;
    public string Key => $"post:page:trending:{IdCursor}:{timeCursor}:{ScoreCursor}:{PageSize}";
    public TimeSpan? CacheExpiryTime => throw new NotImplementedException();
}
