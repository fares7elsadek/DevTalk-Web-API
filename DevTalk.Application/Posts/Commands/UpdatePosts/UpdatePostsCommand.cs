using MediatR;

namespace DevTalk.Application.Posts.Commands.UpdatePosts;

public class UpdatePostsCommand:IRequest
{
    public string PostId { get; set; } = default!;
    public string? Title { get; set; }
    public string? Body { get; set; }
}
