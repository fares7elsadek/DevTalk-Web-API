using DevTalk.Domain.Constants;
using DevTalk.Domain.Entites;

namespace DevTalk.Application.PostVote.Dtos;

public class PostVotesDto
{
    public string VoteId { get; set; } = default!;
    public string VoteType { get; set; } = default!;
    public string PostId { get; set; } = default!;
    public string UserId { get; set; } = default!;
}
