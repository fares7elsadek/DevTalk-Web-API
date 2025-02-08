using MediatR;

namespace DevTalk.Application.Posts.Commands.DeletePost;

public class DeletePostEvent(string id):INotification
{
    public string PostId { get; set; } = id;
}
