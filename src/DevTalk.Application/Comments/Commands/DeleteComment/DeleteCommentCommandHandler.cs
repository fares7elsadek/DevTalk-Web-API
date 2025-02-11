using DevTalk.Application.ApplicationUser;
using DevTalk.Application.Posts;
using DevTalk.Domain.Constants;
using DevTalk.Domain.Exceptions;
using DevTalk.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace DevTalk.Application.Comments.Commands.DeleteComment;

public class DeleteCommentCommandHandler(IUnitOfWork unitOfWork,
    IUserContext userContext,IPublisher publisher) : IRequestHandler<DeleteCommentCommand>
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
            if (user.userId != comment.UserId)
            {
                if(user.userId != post.UserId)
                    throw new CustomeException("User not authorized");
            }
                
        }
        post.Comments.Remove(comment);
        post.PopularityScore = UpdatePostScore.UpdateScore(post);
        await unitOfWork.SaveAsync();
        await publisher.Publish(new DeleteCommentEvent(request.CommentId, request.PostId));
    }
}
