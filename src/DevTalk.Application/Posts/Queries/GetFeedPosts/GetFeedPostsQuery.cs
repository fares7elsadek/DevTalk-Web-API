using DevTalk.Application.Caching;
using DevTalk.Application.Posts.Dtos;

namespace DevTalk.Application.Posts.Queries.GetFeedPosts;

public class GetFeedPostsQuery(string userId) : ICachableRequest<IEnumerable<PostDto>>
{
    public string UserId { get; set; } = userId;
    public string Key => $"feed:user:{UserId}";
    public TimeSpan? CacheExpiryTime => throw new NotImplementedException();
}
