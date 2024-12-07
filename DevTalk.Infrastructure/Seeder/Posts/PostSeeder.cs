using DevTalk.Domain.Entites;
using DevTalk.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using System;

namespace DevTalk.Infrastructure.Seeder.Posts;

public class PostSeeder : IPostSeeder
{
    private readonly AppDbContext _db;
    private readonly UserManager<User> _userManager;
    private readonly string _guid = Guid.NewGuid().ToString();

    public PostSeeder(AppDbContext db, UserManager<User> userManager)
    {
        _db = db;
        _userManager = userManager;
    }

    public async Task Seed()
    {
        if (await _db.Database.CanConnectAsync())
        {
            if (!_db.Users.Any())
            {
                var user = new User
                {
                    FirstName = "Fares",
                    LastName = "Elsadek",
                    Email = "fares@gmail.com",
                    UserName= "Fares",
                    Id = _guid
                };
                var password = "D9{.t(-3`G2J";
                var result = await _userManager.CreateAsync(user, password);

                
            }

            if (!_db.Posts.Any())
            {
                var posts = GetPosts();
                _db.Posts.AddRange(posts);
                await _db.SaveChangesAsync();
            }
        }
    }

    public IEnumerable<Post> GetPosts()
    {
        return new List<Post>
        {
            new Post() {UserId=_guid , PostId = Guid.NewGuid().ToString(), Title = "Second Post", Body = "This is the body of the second post.", PostedAt = DateTime.Now },
            new Post() {UserId=_guid , PostId = Guid.NewGuid().ToString(), Title = "Third Post", Body = "This is the body of the third post.", PostedAt = DateTime.Now },
            new Post() {UserId=_guid , PostId = Guid.NewGuid().ToString(), Title = "Fourth Post", Body = "This is the body of the fourth post.", PostedAt = DateTime.Now },
            new Post() {UserId=_guid , PostId = Guid.NewGuid().ToString(), Title = "Fifth Post", Body = "This is the body of the fifth post.", PostedAt = DateTime.Now },
            new Post() {UserId=_guid , PostId = Guid.NewGuid().ToString(), Title = "Sixth Post", Body = "This is the body of the sixth post.", PostedAt = DateTime.Now },
            new Post() {UserId=_guid , PostId = Guid.NewGuid().ToString(), Title = "Seventh Post", Body = "This is the body of the seventh post.", PostedAt = DateTime.Now },
            new Post() {UserId=_guid , PostId = Guid.NewGuid().ToString(), Title = "Eighth Post", Body = "This is the body of the eighth post.", PostedAt = DateTime.Now },
            new Post() {UserId=_guid , PostId = Guid.NewGuid().ToString(), Title = "Ninth Post", Body = "This is the body of the ninth post.", PostedAt = DateTime.Now },
            new Post() {UserId=_guid , PostId = Guid.NewGuid().ToString(), Title = "Tenth Post", Body = "This is the body of the tenth post.", PostedAt = DateTime.Now },
            new Post() {UserId=_guid , PostId = Guid.NewGuid().ToString(), Title = "Eleventh Post", Body = "This is the body of the eleventh post.", PostedAt = DateTime.Now },
            new Post() {UserId=_guid , PostId = Guid.NewGuid().ToString(), Title = "First Post", Body = "This is the body of the first post.", PostedAt = DateTime.Now },
        };
    }
}
