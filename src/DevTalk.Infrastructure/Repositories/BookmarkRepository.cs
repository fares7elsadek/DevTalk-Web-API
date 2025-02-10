using DevTalk.Domain.Entites;
using DevTalk.Domain.Repositories;
using DevTalk.Infrastructure.Data;

namespace DevTalk.Infrastructure.Repositories;

public class BookmarkRepository(AppDbContext _db):Repository<Bookmarks>(_db),IBookmarkRepository
{
}
