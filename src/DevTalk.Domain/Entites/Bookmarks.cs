namespace DevTalk.Domain.Entites;

public class Bookmarks
{
    public string BookmarkId { get; set; } = default!;
    public DateTime CreatedDate { get; set; }
    public string UserId { get; set; } = default!;
    public User User { get; set; } = default!;
    public string PostId { get; set; } = default!;
    public Post Post { get; set; } = default!;
}
