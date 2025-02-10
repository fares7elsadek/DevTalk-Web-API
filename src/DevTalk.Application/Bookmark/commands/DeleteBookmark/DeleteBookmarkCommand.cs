using MediatR;

namespace DevTalk.Application.Bookmark.commands.DeleteBookmark;

public class DeleteBookmarkCommand(string postId):IRequest
{
    public string PostId { get; set; } = postId;
}
