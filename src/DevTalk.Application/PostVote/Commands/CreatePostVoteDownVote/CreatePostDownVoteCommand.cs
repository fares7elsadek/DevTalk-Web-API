using MediatR;

namespace DevTalk.Application.PostVote.Commands.CreatePostVoteDownVote;

public class CreatePostDownVoteCommand(string postId):IRequest
{
    public string PostId { get; set; } = postId;
}
