using DevTalk.Application.ApplicationUser.Dtos;
using DevTalk.Application.Posts.Dtos;
using DevTalk.Domain.Entites;
using Humanizer;

namespace DevTalk.Application.Comments.Dtos;

public class CommentDto
{
    public string CommentId { get; set; } = default!;
    public string CommentText { get; set; } = default!;
    public DateTime CommentedAt { get; set; }
    public string CommentedAtAgo => CommentedAt.Humanize();
    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedAtAgo => UpdatedAt?.Humanize();
    public ICollection<CommentVotes> Votes { get; set; }
    public string PostId { get; set; } = default!;
    public string UserId { get; set; } = default!;
    public UserDto User { get; set; } = default!;

}
