using DevTalk.Application.ApplicationUser;
using DevTalk.Domain.Constants;
using DevTalk.Domain.Exceptions;
using DevTalk.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace DevTalk.Application.Comments.Commands.DeleteComment;

public class DeleteCommentCommandHandler(IUnitOfWork unitOfWork,
    IUserContext userContext) : IRequestHandler<DeleteCommentCommand>
{
    public async Task Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
    {
        if (request.PostId == null || request.CommentId ==null)
            throw new ArgumentNullException("id");

        var user = userContext.GetCurrentUser();
        var post = await unitOfWork.Post.GetOrDefalutAsync(d => d.PostId == request.PostId,
            IncludeProperties: "Comments,User");
      
        if (post == null)
            throw new NotFoundException(nameof(post), request.PostId);

        var comment = post.Comments.SingleOrDefault(c => c.CommentId == request.CommentId);

        if(comment == null)
            throw new NotFoundException(nameof(comment), request.CommentId);

        if (!user.IsInRole(UserRoles.Admin))
        {
            if (user.userId != comment.UserId || user.userId != post.UserId)
                throw new CustomeException("User not authorized");
        }
        unitOfWork.Comment.Remove(comment);
        await unitOfWork.SaveAsync();
    }
}
