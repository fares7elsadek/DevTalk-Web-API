using DevTalk.Domain.Entites;
using MediatR;

namespace DevTalk.Application.Comments.Commands.CreateComment;

public class CreateCommentEvent(string postId):INotification
{
    public string CommentText { get; set; } = default!;
    public string PostId { get; set; } = postId;
}
