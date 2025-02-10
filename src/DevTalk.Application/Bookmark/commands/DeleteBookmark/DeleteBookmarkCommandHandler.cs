using DevTalk.Application.ApplicationUser;
using DevTalk.Domain.Exceptions;
using DevTalk.Domain.Repositories;
using MediatR;

namespace DevTalk.Application.Bookmark.commands.DeleteBookmark;

public class DeleteBookmarkCommandHandler(IUserContext userContext,
    IUnitOfWork unitOfWork,IPublisher publisher) : IRequestHandler<DeleteBookmarkCommand>
{
    public async Task Handle(DeleteBookmarkCommand request, CancellationToken cancellationToken)
    {
        var user = userContext.GetCurrentUser();
        if (user == null)
            throw new CustomeException("User not authorized");

        var post = await unitOfWork.Post.GetOrDefalutAsync(x => x.PostId == request.PostId);
        if (post == null)
            throw new NotFoundException(nameof(post), request.PostId);

        var appUser = await unitOfWork.User.GetOrDefalutAsync(x => x.Id == user.userId,
            IncludeProperties: "Bookmarks");

        if (appUser == null)
            throw new CustomeException("Somthing wrong has happened");

        var bookmark = appUser.Bookmarks.FirstOrDefault(x => x.PostId == request.PostId);
        if (bookmark is null)
            throw new CustomeException("Post is not in the bookmarks list");

        unitOfWork.Bookmark.Remove(bookmark);
        await unitOfWork.SaveAsync();
        await publisher.Publish(new DeleteBookmarkEvent(request.PostId,user.userId));
    }
}
