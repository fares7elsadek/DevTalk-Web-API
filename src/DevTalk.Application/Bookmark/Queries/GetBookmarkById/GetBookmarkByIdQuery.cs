using DevTalk.Application.Bookmark.Dtos;
using DevTalk.Application.Caching;
using MediatR;

namespace DevTalk.Application.Bookmark.Queries.GetBookmarkById;

public class GetBookmarkByIdQuery(string userId,string bookmarkId) : IRequest<BookmarkDto>
{
    public string UserId { get; set; } = userId;
    public string BookmarkId { get; set; } = bookmarkId;
}
