using DevTalk.Application.Posts.Commands.CreatePosts;
using DevTalk.Application.Posts.Commands.DeletePost;
using DevTalk.Application.Posts.Commands.UpdatePosts;
using DevTalk.Application.Services.Caching;
using MediatR;
using System.Reflection.Metadata;

namespace DevTalk.Application.Posts;

public class CacheInvalidationPostHandler(ICachingService cache) : INotificationHandler<CreatePostsEvent>,
    INotificationHandler<UpdatePostEvent>,
    INotificationHandler<DeletePostEvent>
{
    public async Task Handle(DeletePostEvent notification, CancellationToken cancellationToken)
    {
        await cache.RemoveData($"post:{notification.PostId}");
        await InternalHandler();
    }

    public async Task Handle(UpdatePostEvent notification, CancellationToken cancellationToken)
    {
        await cache.RemoveData($"post:{notification.PostId}");
        await InternalHandler();
    }

    public Task Handle(CreatePostsEvent notification, CancellationToken cancellationToken)
    {
        return InternalHandler();
    }

    public async Task InternalHandler()
    {
        await cache.RemoveData("post:all");
        await cache.RemoveData("post:trending");
    }
}
