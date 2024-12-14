using DevTalk.Application.Comments.Dtos;
using DevTalk.Application.PostMedias.Dtos;
using DevTalk.Application.PostVote.Dtos;
using DevTalk.Domain.Entites;

namespace DevTalk.Application.Posts.Dtos;

public class PostDto
{
    public string PostId { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string Body { get; set; } = default!;
    public DateTime PostedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public ICollection<PostMediasDto>? PostMedias { get; set; }
    public ICollection<PostVotesDto>? Votes { get; set; }
    public string UserId { get; set; } = default!;
    public string Username { get; set; } = default!;

}
