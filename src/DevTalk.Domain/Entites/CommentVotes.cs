using DevTalk.Domain.Constants;

namespace DevTalk.Domain.Entites;

public class CommentVotes
{
    public string VoteId { get; set; } = default!;
    public VoteType VoteType { get; set; }
    public string CommentId { get; set; } = default!;
    public Comment Comment { get; set; } = default!;
    public string UserId { get; set; }  = default!;
    public User User { get; set; } = default!;
}
