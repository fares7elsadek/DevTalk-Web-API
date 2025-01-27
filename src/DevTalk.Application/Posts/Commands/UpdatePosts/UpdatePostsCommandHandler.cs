using DevTalk.Application.ApplicationUser;
using DevTalk.Domain.Constants;
using DevTalk.Domain.Exceptions;
using DevTalk.Domain.Repositories;
using MediatR;

namespace DevTalk.Application.Posts.Commands.UpdatePosts;

public class UpdatePostsCommandHandler(IUnitOfWork unitOfWork,
    IUserContext userContext) : IRequestHandler<UpdatePostsCommand>
{
    public async Task Handle(UpdatePostsCommand request, CancellationToken cancellationToken)
    {
        if(string.IsNullOrWhiteSpace(request.Title) && string.IsNullOrWhiteSpace(request.Body)) 
            throw new CustomeException("You must update at least one field");

        var id = request.PostId;
        if (id == null)
            throw new ArgumentNullException("id");
        var user = userContext.GetCurrentUser();
        var post = await unitOfWork.Post.GetOrDefalutAsync(d => d.PostId == id,
            IncludeProperties: "User");
        if (post == null)
            throw new NotFoundException(nameof(post), id);

        if (!user.IsInRole(UserRoles.Admin))
        {
            var PostUserId = post.User.Id;
            if (user.userId != PostUserId)
                throw new CustomeException("User not authroized");
        }

        
        if(!string.IsNullOrWhiteSpace(request.Title))
            post.Title = request.Title;
        if(!string.IsNullOrWhiteSpace(request.Body))
            post.Body = request.Body;
        unitOfWork.Post.Update(post);
        await unitOfWork.SaveAsync();
    }
}
