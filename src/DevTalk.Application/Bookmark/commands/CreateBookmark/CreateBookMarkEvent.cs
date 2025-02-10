using MediatR;

namespace DevTalk.Application.Bookmark.commands.CreateBookmark;

public class CreateBookMarkEvent(string postId,string userId):INotification
{
    public string PostId { get; set; } = postId;
    public string UserId { get; set; } = userId;
}
