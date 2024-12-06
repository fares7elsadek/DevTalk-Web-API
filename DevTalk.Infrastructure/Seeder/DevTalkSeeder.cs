
using DevTalk.Infrastructure.Data;
using DevTalk.Infrastructure.Seeder.Identity;
using DevTalk.Infrastructure.Seeder.Posts;

namespace DevTalk.Infrastructure.Seeder;

public class DevTalkSeeder(AppDbContext db) : IDevTalkSeeder
{
    public IIdentitySeeder IdentitySeederObject => new IdentitySeeder(db);

    public IPostSeeder PostSeeder => new PostSeeder(db);

    async Task IDevTalkSeeder.Seed()
    {
        await IdentitySeederObject.Seed();
        await PostSeeder.Seed();
    }
}
