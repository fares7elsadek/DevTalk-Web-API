using DevTalk.Application.Attributes;
using DevTalk.Domain.Helpers;
using MediatR;

namespace DevTalk.Application.Comments.Commands.UpdateComment;

[HasPermission(Permissions.UpdateComment)]
public class UpdateCommentCommand(string commentId,string postId):IRequest,ICommentCommand<string>
{
    public string CommentText { get; set; } = default!;
    public string? CommentId { get; set; } = commentId;
    public string? PostId { get; set; } = postId;
    public string ResourceId => CommentId!;
}
