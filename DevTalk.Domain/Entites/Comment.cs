namespace DevTalk.Domain.Entites;

public class Comment
{
    public Comment()
    {
        Votes = new HashSet<CommentVotes>();
    }
    public string CommentId { get; set; } = default!;
    public string CommentText { get; set; } = default!;
    public DateTime CommentedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public ICollection<CommentVotes> Votes { get; set; }
    public string PostId { get; set; } = default!;
    public Post Post { get; set; } = default!;
    public string UserId { get; set; } = default!;
    public User User { get; set; } = default!;
}
