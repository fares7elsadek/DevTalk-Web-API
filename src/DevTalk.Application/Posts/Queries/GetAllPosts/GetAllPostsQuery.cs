using DevTalk.Application.Caching;
using DevTalk.Application.Posts.Dtos;
using MediatR;

namespace DevTalk.Application.Posts.Queries.GetAllPosts;

public class GetAllPostsQuery : ICachableRequest<IEnumerable<PostDto>>
{
    public string Key => "post:all";
    public TimeSpan? CacheExpiryTime => throw new NotImplementedException();
}
