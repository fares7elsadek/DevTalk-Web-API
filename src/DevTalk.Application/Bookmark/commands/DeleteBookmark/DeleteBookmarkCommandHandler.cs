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
        string userId = userContext.GetCurrentUser().userId;

        var bookmark = await unitOfWork.Bookmark
            .GetOrDefalutAsync(x => x.PostId == request.PostId && x.UserId == userId);

        if (bookmark is null)
            throw new CustomeException("Post is not in the bookmarks list");

        unitOfWork.Bookmark.Remove(bookmark);
        await unitOfWork.SaveAsync();

        await publisher.Publish(new DeleteBookmarkEvent(request.PostId, userId), cancellationToken);
    }
}
