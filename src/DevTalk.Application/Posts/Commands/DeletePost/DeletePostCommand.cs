using DevTalk.Application.Attributes;
using DevTalk.Domain.Helpers;
using MediatR;

namespace DevTalk.Application.Posts.Commands.DeletePost;

[HasPermission(Permissions.DeletePost)]
public class DeletePostCommand(string id):IRequest,IPostCommand<string>
{
    public string PostId { get; set; } = id;
    public string ResourceId  => PostId;
}
