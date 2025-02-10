namespace DevTalk.Domain.Entites;

public class Post
{
    public Post()
    {
        PostMedias = new HashSet<PostMedia>();
        Votes = new HashSet<PostVotes>();
        Comments = new HashSet<Comment>();
        Categories = new HashSet<Categories>();
        PostCategories = new HashSet<PostCategory>();
        Bookmarks = new HashSet<Bookmarks>();
    }
    public string PostId { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string Body { get; set; } = default!;
    public DateTime PostedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public ICollection<PostMedia> PostMedias { get; set; }
    public ICollection<PostVotes> Votes { get; set; }
    public ICollection<Comment> Comments { get; set; }
    public string UserId { get; set; } = default!;
    public User User { get; set; } = default!;
    public ICollection<Categories> Categories { get; set; }
    public ICollection<PostCategory> PostCategories { get; set; }
    public ICollection<Bookmarks> Bookmarks { get; set; }
}
