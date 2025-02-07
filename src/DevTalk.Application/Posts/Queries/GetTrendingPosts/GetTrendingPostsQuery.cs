using DevTalk.Application.Caching;
using DevTalk.Application.Posts.Dtos;
using MediatR;

namespace DevTalk.Application.Posts.Queries.GetTrendingPosts;

public class GetTrendingPostsQuery(int page,int size):ICachableRequest<IEnumerable<PostDto>>
{
    public int page { get; set; } = page;
    public int size { get; set; } = size;
    public string Key => "post:trending";
    public TimeSpan? CacheExpiryTime => throw new NotImplementedException();
}
