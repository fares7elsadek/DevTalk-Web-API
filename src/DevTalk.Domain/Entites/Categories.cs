namespace DevTalk.Domain.Entites;

public class Categories
{
    public Categories()
    {
        Posts = new HashSet<Post>();
        PostCategories = new HashSet<PostCategory>();
        Preferences = new HashSet<Preference>();
    }
    public string CategoryId { get; set; } = default!;
    public string CategoryName { get; set; } = default!;
    public ICollection<Post> Posts { get; set; }
    public ICollection<PostCategory> PostCategories { get; set; }
    public ICollection<Preference> Preferences { get; set; }
}
