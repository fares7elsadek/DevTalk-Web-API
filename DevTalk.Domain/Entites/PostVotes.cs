using DevTalk.Domain.Constants;

namespace DevTalk.Domain.Entites;

public class PostVotes
{
    public string VoteId { get; set; } = default!;
    public VoteType VoteType { get; set; }
    public string PostId { get; set; } = default!;
    public Post Post { get; set; } = default!;
    public string UserId { get; set; } = default!;
    public User User { get; set; } = default!;
}
