using MediatR;

namespace DevTalk.Application.Comments.Commands.UpdateComment;

public class UpdateCommentCommand(string commentId,string postId):IRequest
{
    public string CommentText { get; set; } = default!;
    public string? CommentId { get; set; } = commentId;
    public string? PostId { get; set; } = postId;
}
