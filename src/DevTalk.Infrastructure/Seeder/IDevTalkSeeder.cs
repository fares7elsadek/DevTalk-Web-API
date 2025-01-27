using DevTalk.Infrastructure.Seeder.Identity;
using DevTalk.Infrastructure.Seeder.Posts;


namespace DevTalk.Infrastructure.Seeder;

public interface IDevTalkSeeder
{
    public IIdentitySeeder IdentitySeederObject { get; }
    public IPostSeeder PostSeeder { get; }
    public Task Seed();
}
