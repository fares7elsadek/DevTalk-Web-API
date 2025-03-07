using DevTalk.Application.Attributes;
using DevTalk.Domain.Helpers;
using MediatR;

namespace DevTalk.Application.Posts.Commands.UpdatePosts;

[HasPermission(Permissions.UpdatePost)]
public class UpdatePostsCommand:IRequest,IPostCommand<string>
{
    public string PostId { get; set; } = default!;
    public string? Title { get; set; }
    public string? Body { get; set; }

    public string ResourceId => PostId;
}
