using DevTalk.Application.ApplicationUser;
using DevTalk.Domain.Exceptions;
using DevTalk.Domain.Repositories;
using MediatR;

namespace DevTalk.Application.Comments.Commands.UpdateComment;

public class UpdateCommentCommandHandler(IUnitOfWork unitOfWork
    ,IPublisher publisher) : IRequestHandler<UpdateCommentCommand>
{
    public async Task Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
    {
        if (request.PostId == null || request.CommentId == null)
            throw new ArgumentNullException(nameof(request));


        var comment = await unitOfWork.Comment.GetOrDefalutAsync(d => 
        d.PostId == request.PostId && d.CommentId == request.CommentId);

        if (comment == null)
            throw new NotFoundException(nameof(comment), request.CommentId);

        comment.CommentText = request.CommentText;
        comment.UpdatedAt = DateTime.UtcNow;
        await unitOfWork.SaveAsync();
        await publisher.Publish(new UpdateCommentEvent(request.CommentId, request.PostId));
    }
}
