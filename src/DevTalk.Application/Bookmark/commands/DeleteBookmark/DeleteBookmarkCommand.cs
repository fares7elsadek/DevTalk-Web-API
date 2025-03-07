using DevTalk.Application.Attributes;
using DevTalk.Domain.Helpers;
using MediatR;

namespace DevTalk.Application.Bookmark.commands.DeleteBookmark;

[HasPermission(Permissions.DeleteBookmark)]
public class DeleteBookmarkCommand(string postId):IRequest
{
    public string PostId { get; set; } = postId;
}
