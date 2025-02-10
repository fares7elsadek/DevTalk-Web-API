using DevTalk.Application.ApplicationUser;
using DevTalk.Domain.Entites;
using DevTalk.Domain.Exceptions;
using DevTalk.Domain.Repositories;
using MediatR;

namespace DevTalk.Application.Bookmark.commands.CreateBookmark;

public class CreateBookmarkCommandHandler(IUnitOfWork unitOfWork,
    IUserContext userContext,IPublisher publisher) : IRequestHandler<CreateBookmarkCommand>
{
    public async Task Handle(CreateBookmarkCommand request, CancellationToken cancellationToken)
    {
        var user = userContext.GetCurrentUser();
        if (user == null)
            throw new CustomeException("User not authorized");

        var post = await unitOfWork.Post.GetOrDefalutAsync(x => x.PostId == request.PostId);
        if(post == null)
            throw new NotFoundException(nameof(post),request.PostId);

        var appUser = await unitOfWork.User.GetOrDefalutAsync(x => x.Id == user.userId,
            IncludeProperties: "Bookmarks");

        if (appUser == null)
            throw new CustomeException("Somthing wrong has happened");

        if (appUser.Bookmarks.Any(x => x.PostId == request.PostId))
            throw new CustomeException("Post already added to bookmarks");

        var newBookmark = new Bookmarks { PostId = request.PostId , UserId = user.userId };
        appUser.Bookmarks.Add(newBookmark);
        await unitOfWork.SaveAsync();
        await publisher.Publish(new CreateBookMarkEvent(request.PostId,user.userId));
    }
}
