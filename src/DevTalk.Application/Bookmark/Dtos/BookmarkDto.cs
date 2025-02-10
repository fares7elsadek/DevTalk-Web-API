namespace DevTalk.Application.Bookmark.Dtos;

public class BookmarkDto
{
    public string UserId { get; set; } = default!;
    public string PostId { get; set; } = default!;
    public DateTime CreatedDate { get; set; }
}
