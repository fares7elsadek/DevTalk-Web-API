using DevTalk.Domain.Entites;
using DevTalk.Domain.Repositories;
using DevTalk.Infrastructure.Data;

namespace DevTalk.Infrastructure.Repositories;

public class PreferenceRepository(AppDbContext db) : Repository<Preference>(db), IPreferenceRepository
{
    public async Task AddRangeAsync(IEnumerable<Preference> preferences)
    {
        await db.Preferences.AddRangeAsync(preferences);
    }
}
