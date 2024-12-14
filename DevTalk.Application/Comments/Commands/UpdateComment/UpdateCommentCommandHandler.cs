using DevTalk.Application.ApplicationUser;
using DevTalk.Domain.Exceptions;
using DevTalk.Domain.Repositories;
using MediatR;

namespace DevTalk.Application.Comments.Commands.UpdateComment;

public class UpdateCommentCommandHandler(IUnitOfWork unitOfWork,
    IUserContext userContext) : IRequestHandler<UpdateCommentCommand>
{
    public async Task Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
    {
        if (request.PostId == null || request.CommentId == null)
            throw new ArgumentNullException("id");

        var user = userContext.GetCurrentUser();
        var post = await unitOfWork.Post.GetOrDefalutAsync(d => d.PostId == request.PostId,
            IncludeProperties: "Comments,User");

        if (post == null)
            throw new NotFoundException(nameof(post), request.PostId);

        var comment = post.Comments.SingleOrDefault(c => c.CommentId == request.CommentId);

        if (comment == null)
            throw new NotFoundException(nameof(comment), request.CommentId);
        
        var CommentUserId = comment.UserId;
        if (CommentUserId != user.userId)
            throw new CustomeException("Not authorized");

        comment.CommentText = request.CommentText;
        comment.UpdatedAt = DateTime.UtcNow;
        await unitOfWork.SaveAsync();
    }
}
