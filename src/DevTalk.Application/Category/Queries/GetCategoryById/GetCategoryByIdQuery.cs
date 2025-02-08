using DevTalk.Application.Caching;
using DevTalk.Application.Category.Dtos;

namespace DevTalk.Application.Category.Queries.GetCategoryById;

public class GetCategoryByIdQuery(string categoryId) : ICachableRequest<CategoryDto>
{
    public string CategoryId { get; set; } = categoryId;
    public string Key => $"category:{CategoryId}";
    public TimeSpan? CacheExpiryTime => throw new NotImplementedException();
}
