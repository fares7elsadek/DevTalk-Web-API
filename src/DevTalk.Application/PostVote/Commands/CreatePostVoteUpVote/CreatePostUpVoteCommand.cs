using MediatR;

namespace DevTalk.Application.PostVote.Commands.CreatePostVoteUpVote;

public class CreatePostUpVoteCommand(string postId):IRequest
{
    public string PostId { get; set; } = postId;
}
