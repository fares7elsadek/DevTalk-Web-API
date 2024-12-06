using Microsoft.AspNetCore.Identity;

namespace DevTalk.Domain.Entites;

public class User:IdentityUser
{
    public User()
    {
        Posts = new HashSet<Post>();
        Comments = new HashSet<Comment>();
        PostVotes = new HashSet<PostVotes>();
        CommentsVotes = new HashSet<CommentVotes>();
    }
    public string? FirstName { get; set; }
    public string? LastName { get; set; } 
    public string? Avatar { get; set; }
    public DateOnly? BirthOfDate { get; set; }
    public ICollection<Post> Posts { get; set; }
    public ICollection<Comment> Comments { get; set; }
    public ICollection<PostVotes> PostVotes { get; set; }
    public ICollection<CommentVotes> CommentsVotes { get; set; }
}
