using MediatR;

namespace DevTalk.Application.Comments.Commands.CreateComment;

public class CreateCommentCommand(string postId):IRequest
{
    public string CommentText { get; set; } = default!;
    public string? PostId { get; set; } = postId;
}
