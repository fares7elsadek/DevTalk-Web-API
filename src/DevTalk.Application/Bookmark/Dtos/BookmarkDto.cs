using DevTalk.Application.Posts.Dtos;
using DevTalk.Domain.Entites;

namespace DevTalk.Application.Bookmark.Dtos;

public class BookmarkDto
{
    public PostDto Post { get; set; } = default!;
    public DateTime CreatedDate { get; set; }
}
