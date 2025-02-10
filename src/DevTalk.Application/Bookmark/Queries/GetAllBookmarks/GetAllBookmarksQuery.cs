using DevTalk.Application.Bookmark.Dtos;
using DevTalk.Application.Caching;

namespace DevTalk.Application.Bookmark.Queries.GetAllBookmarks;

public class GetAllBookmarksQuery(string userId) : ICachableRequest<IEnumerable<BookmarkDto>>
{
    public string UserId { get; set; } = userId;
    public string Key => $"bookmarks:user:{UserId}:all";
    public TimeSpan? CacheExpiryTime => throw new NotImplementedException();
}
