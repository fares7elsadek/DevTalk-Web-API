using MediatR;

namespace DevTalk.Application.Comments.Commands.DeleteComment;

public class DeleteCommentCommand(string commentId,string postId):IRequest
{
    public string CommentId { get; set; } = commentId;
    public string? PostId { get; set;} = postId;
}
