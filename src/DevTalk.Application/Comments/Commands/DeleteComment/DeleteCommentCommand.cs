using DevTalk.Application.Attributes;
using DevTalk.Domain.Helpers;
using MediatR;

namespace DevTalk.Application.Comments.Commands.DeleteComment;

[HasPermission(Permissions.DeleteComment)]
public class DeleteCommentCommand(string commentId,string postId):IRequest,ICommentCommand<string>
{
    public string CommentId { get; set; } = commentId;
    public string? PostId { get; set;} = postId;

    public string ResourceId => CommentId;
}
