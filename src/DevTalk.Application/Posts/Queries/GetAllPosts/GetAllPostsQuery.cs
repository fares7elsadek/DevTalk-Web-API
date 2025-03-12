using DevTalk.Application.Caching;
using DevTalk.Application.Posts.Dtos;
using MediatR;

namespace DevTalk.Application.Posts.Queries.GetAllPosts;

public class GetAllPostsQuery(int pageSize,string cursor) : ICachableRequest<GetAllPostsDto>
{
    public int PageSize { get; set; } = pageSize;
    public string Cursor { get; set; } = cursor;
    public string Key => $"post:page:{Cursor}:{pageSize}";
    public TimeSpan? CacheExpiryTime => throw new NotImplementedException();
}
