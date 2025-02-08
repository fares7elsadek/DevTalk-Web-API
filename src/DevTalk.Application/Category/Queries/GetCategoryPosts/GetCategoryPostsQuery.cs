using DevTalk.Application.Caching;
using DevTalk.Application.Category.Dtos;
using DevTalk.Application.Posts.Dtos;

namespace DevTalk.Application.Category.Queries.GetCategoryPosts;

public class GetCategoryPostsQuery(string categoryId):ICachableRequest<IEnumerable<PostDto>>
{
    public string CategoryId { get; set; } = categoryId;
    public string Key => $"category:{CategoryId}:posts";
    public TimeSpan? CacheExpiryTime => throw new NotImplementedException();
}
