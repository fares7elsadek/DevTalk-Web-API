using DevTalk.Domain.Entites;
using MediatR;

namespace DevTalk.Application.Comments.Commands.UpdateComment;

public class UpdateCommentEvent(string commentId,string postId):INotification
{
    public string? CommentId { get; set; } = commentId;
    public string PostId { get; set; } = postId;
}
