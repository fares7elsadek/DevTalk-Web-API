using DevTalk.Application.Caching;
using DevTalk.Application.Posts.Dtos;
using MediatR;

namespace DevTalk.Application.Posts.Queries.GetFeedPosts;

public class GetFeedPostsQuery(string userId,
    string idCursor, string timeCursor
    , double scoreCursor, int pageSize) : ICachableRequest<GetUserPostsDto>
{
    public string IdCursor { get; set; } = idCursor;
    public string timeCursor { get; set; } = timeCursor;
    public double ScoreCursor { get; set; } = scoreCursor;
    public int PageSize { get; set; } = pageSize;
    public string UserId { get; set; } = userId;
    public string Key => $"feed:user:{UserId}:{IdCursor}:{timeCursor}:{PageSize}";
    public TimeSpan? CacheExpiryTime => throw new NotImplementedException();
}
