using DevTalk.Application.Caching;
using DevTalk.Application.Posts.Dtos;
using MediatR;

namespace DevTalk.Application.Posts.Queries.GetPostById;

public class GetPostByIdQuery(string Id):ICachableRequest<PostDto>
{
    public string PostId { get; set; } = Id;

    public string Key => $"post:{PostId}";

    public TimeSpan? CacheExpiryTime => throw new NotImplementedException();
}
