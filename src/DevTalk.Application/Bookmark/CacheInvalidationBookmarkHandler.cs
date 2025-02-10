using DevTalk.Application.Bookmark.commands.CreateBookmark;
using DevTalk.Application.Bookmark.commands.DeleteBookmark;
using DevTalk.Application.Services.Caching;
using MediatR;

namespace DevTalk.Application.Bookmark;

public class CacheInvalidationBookmarkHandler(ICachingService cache) : INotificationHandler<CreateBookMarkEvent>,
    INotificationHandler<DeleteBookmarkEvent>
{
    public Task Handle(CreateBookMarkEvent notification, CancellationToken cancellationToken)
    {
        return InternalHandler(notification.UserId);
    }
    public Task Handle(DeleteBookmarkEvent notification, CancellationToken cancellationToken)
    {
        return InternalHandler(notification.UserId);
    }

    public async Task InternalHandler(string userId)
    {
        await cache.RemoveData($"bookmarks:user:{userId}:all");
    }
}
