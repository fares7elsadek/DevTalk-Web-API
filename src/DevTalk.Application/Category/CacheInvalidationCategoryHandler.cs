using DevTalk.Application.Category.Commands.CreateCategory;
using DevTalk.Application.Category.Commands.DeleteCategory;
using DevTalk.Application.Services.Caching;
using MediatR;

namespace DevTalk.Application.Category;

public class CacheInvalidationCategoryHandler(ICachingService cache) :
    INotificationHandler<CreateCategoryEvent>,
    INotificationHandler<DeleteCategoryEvent>
{
    public Task Handle(CreateCategoryEvent notification, CancellationToken cancellationToken)
    {
        return InernalHandler();
    }

    public async Task Handle(DeleteCategoryEvent notification, CancellationToken cancellationToken)
    {
        await cache.RemoveData($"category:{notification.CategoryId}");
        await InernalHandler();
    }
    
    public async Task InernalHandler()
    {
        await cache.RemoveData("category:all");
    }
}
