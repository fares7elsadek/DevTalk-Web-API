using DevTalk.Application.Comments.Commands.CreateComment;
using DevTalk.Application.Comments.Commands.DeleteComment;
using DevTalk.Application.Comments.Commands.UpdateComment;
using DevTalk.Application.Services.Caching;
using MediatR;

namespace DevTalk.Application.Comments;

public class CacheInvalidationCommentHandler(ICachingService cache) :
    INotificationHandler<CreateCommentEvent>,
    INotificationHandler<DeleteCommentEvent>,
    INotificationHandler<UpdateCommentEvent>
{
    public Task Handle(CreateCommentEvent notification, CancellationToken cancellationToken)
    {
        return InternalHandler(notification.PostId);
    }

    public async Task Handle(DeleteCommentEvent notification, CancellationToken cancellationToken)
    {
        await cache.RemoveData($"post:{notification.PostId}:comment:{notification.CommentId}");
        await InternalHandler(notification.PostId);
    }

    public async Task Handle(UpdateCommentEvent notification, CancellationToken cancellationToken)
    {
        await cache.RemoveData($"post:{notification.PostId}:comment:{notification.CommentId}");
        await InternalHandler(notification.PostId);
    }

    public async Task InternalHandler(string postId)
    {
        await cache.RemoveData($"post:{postId}:comment:all");
    }
}
