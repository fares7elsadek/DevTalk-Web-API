using MediatR;

namespace DevTalk.Application.Posts.Commands.UpdatePosts;

public class UpdatePostEvent:INotification
{
    public string PostId { get; set; } = default!;
    public string? Title { get; set; }
    public string? Body { get; set; }
}
