using MediatR;

namespace DevTalk.Application.Posts.Commands.DeletePost;

public class DeletePostCommand(string id):IRequest
{
    public string PostId { get; set; } = id;
}
