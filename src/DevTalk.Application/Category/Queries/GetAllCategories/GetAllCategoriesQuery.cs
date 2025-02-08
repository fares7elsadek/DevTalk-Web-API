using DevTalk.Application.Caching;
using DevTalk.Application.Category.Dtos;

namespace DevTalk.Application.Category.Queries.GetAllCategories;

public class GetAllCategoriesQuery : ICachableRequest<IEnumerable<CategoryDto>>
{
    public string Key => "category:all";
    public TimeSpan? CacheExpiryTime => throw new NotImplementedException();
}
