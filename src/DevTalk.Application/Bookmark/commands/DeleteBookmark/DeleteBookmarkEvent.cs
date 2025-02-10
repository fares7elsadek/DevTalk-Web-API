using MediatR;

namespace DevTalk.Application.Bookmark.commands.DeleteBookmark;

public class DeleteBookmarkEvent(string postId,string userId):INotification
{
    public string PostId { get; set; } = postId;
    public string UserId { get; set; } = userId;
}
