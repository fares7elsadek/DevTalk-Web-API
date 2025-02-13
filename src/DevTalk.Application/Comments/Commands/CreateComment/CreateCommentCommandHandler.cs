using DevTalk.Application.ApplicationUser;
using DevTalk.Application.Posts;
using DevTalk.Domain.Constants;
using DevTalk.Domain.Entites;
using DevTalk.Domain.Exceptions;
using DevTalk.Domain.Repositories;
using MediatR;

namespace DevTalk.Application.Comments.Commands.CreateComment;

public class CreateCommentCommandHandler(IUnitOfWork unitOfWork,
    IUserContext userContext,IPublisher publisher) : IRequestHandler<CreateCommentCommand>
{
    public async Task Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {
        var id = request.PostId;
        if (id == null)
            throw new ArgumentNullException("id");
        var user = userContext.GetCurrentUser();
        var post = await unitOfWork.Post.GetOrDefalutAsync(d => d.PostId == id,
            IncludeProperties: "Comments,User");
        if (post == null)
            throw new NotFoundException(nameof(post), id);

        Comment NewComment = new Comment
        {
            CommentText = request.CommentText,
            CommentedAt = DateTime.Now,
            PostId = id,
            UserId = user.userId,
        };
        post.Comments.Add(NewComment);
        post.PopularityScore = UpdatePostScore.UpdateScore(post);
        await unitOfWork.SaveAsync();
        await publisher.Publish(new CreateCommentEvent(NewComment.PostId)
        {
            CommentText = request.CommentText,
        });
    }
}
