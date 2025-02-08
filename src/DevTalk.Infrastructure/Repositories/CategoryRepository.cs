using DevTalk.Domain.Entites;
using DevTalk.Domain.Repositories;
using DevTalk.Infrastructure.Data;

namespace DevTalk.Infrastructure.Repositories;

public class CategoryRepository(AppDbContext _db) : Repository<Categories>(_db), ICategoryRepository
{
}
