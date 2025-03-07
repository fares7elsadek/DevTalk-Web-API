using DevTalk.Application.ApplicationUser;
using DevTalk.Application.Posts;
using DevTalk.Domain.Constants;
using DevTalk.Domain.Exceptions;
using DevTalk.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace DevTalk.Application.Comments.Commands.DeleteComment;

public class DeleteCommentCommandHandler(IUnitOfWork unitOfWork
    ,IPublisher publisher) : IRequestHandler<DeleteCommentCommand>
{
    public async Task Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.PostId) || string.IsNullOrEmpty(request.CommentId))
            throw new ArgumentNullException(nameof(request));

        var comment = await unitOfWork.Comment
            .GetOrDefalutAsync(c => c.CommentId == request.CommentId && c.PostId == request.PostId);

        if (comment is null)
            throw new NotFoundException(nameof(comment), request.CommentId);

        // Remove comment
        unitOfWork.Comment.Remove(comment);

        // Update post score
        var post = await unitOfWork.Post.GetOrDefalutAsync(p => p.PostId == request.PostId);
        if (post != null)
        {
            post.PopularityScore = UpdatePostScore.UpdateScore(post);
        }

        await unitOfWork.SaveAsync();
        await publisher.Publish(new DeleteCommentEvent(request.CommentId, request.PostId), cancellationToken);
    }
}
