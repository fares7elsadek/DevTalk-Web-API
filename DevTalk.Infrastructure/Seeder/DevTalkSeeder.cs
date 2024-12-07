
using DevTalk.Domain.Entites;
using DevTalk.Infrastructure.Data;
using DevTalk.Infrastructure.Seeder.Identity;
using DevTalk.Infrastructure.Seeder.Posts;
using Microsoft.AspNetCore.Identity;

namespace DevTalk.Infrastructure.Seeder;

public class DevTalkSeeder(AppDbContext db,UserManager<User> userManager) : IDevTalkSeeder
{
    public IIdentitySeeder IdentitySeederObject => new IdentitySeeder(db);
    public IPostSeeder PostSeeder => new PostSeeder(db,userManager);
    async Task IDevTalkSeeder.Seed()
    {
        await IdentitySeederObject.Seed();
        await PostSeeder.Seed();
    }
}
