using DevTalk.Domain.Entites;
using MediatR;

namespace DevTalk.Application.Comments.Commands.DeleteComment;

public class DeleteCommentEvent(string commentId,string postId):INotification
{
    public string CommentId { get; set; } = commentId;
    public string PostId { get; set; } = postId;
}
