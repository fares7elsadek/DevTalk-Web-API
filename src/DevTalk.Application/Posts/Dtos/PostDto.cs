using DevTalk.Application.Comments.Dtos;
using DevTalk.Application.PostMedias.Dtos;
using DevTalk.Application.PostVote.Dtos;
using DevTalk.Domain.Entites;
using Humanizer;

namespace DevTalk.Application.Posts.Dtos;

public class PostDto
{
    public string PostId { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string Body { get; set; } = default!;
    public DateTime PostedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string PostedAtAgo => PostedAt.Humanize();
    public string? UpdatedAtAgo => UpdatedAt?.Humanize();
    public ICollection<PostMediasDto>? PostMedias { get; set; }
    public ICollection<PostVotesDto>? Votes { get; set; }
    public int UpVotes { get; set; }
    public int DownVotes { get; set; }
    public int Comments { get; set; }
    public string UserId { get; set; } = default!;
    public string Username { get; set; } = default!;
    public List<string> Categories { get; set; } = new();

}
