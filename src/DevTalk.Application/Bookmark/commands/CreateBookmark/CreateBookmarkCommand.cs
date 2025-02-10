using MediatR;

namespace DevTalk.Application.Bookmark.commands.CreateBookmark;

public class CreateBookmarkCommand(string postId):IRequest
{
    public string PostId { get; set; } = postId;
}
