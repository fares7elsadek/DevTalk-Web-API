
using DevTalk.Domain.Entites;
using DevTalk.Infrastructure.Data;


namespace DevTalk.Infrastructure.Seeder.Posts;

public class PostSeeder(AppDbContext db) : IPostSeeder
{
    public async Task Seed()
    {
        if (await db.Database.CanConnectAsync())
        {
            if (!db.Posts.Any())
            {
                var Posts = GetPosts();
                db.Posts.AddRange(Posts);
                await db.SaveChangesAsync();
            }
        }
    }
    public IEnumerable<Post> GetPosts()
    {
        return new List<Post>
        {
            new Post() {UserId="ef23d9b1-4b6b-4402-80b3-b307c3ba2c8d" , PostId = Guid.NewGuid().ToString(), Title = "First Post", Body = "This is the body of the first post.", PostedAt = DateTime.Now },
            new Post() {UserId="ef23d9b1-4b6b-4402-80b3-b307c3ba2c8d" , PostId = Guid.NewGuid().ToString(), Title = "Second Post", Body = "This is the body of the second post.", PostedAt = DateTime.Now },
            new Post() {UserId="ef23d9b1-4b6b-4402-80b3-b307c3ba2c8d" , PostId = Guid.NewGuid().ToString(), Title = "Third Post", Body = "This is the body of the third post.", PostedAt = DateTime.Now },
            new Post() {UserId="ef23d9b1-4b6b-4402-80b3-b307c3ba2c8d" , PostId = Guid.NewGuid().ToString(), Title = "Fourth Post", Body = "This is the body of the fourth post.", PostedAt = DateTime.Now },
            new Post() {UserId="ef23d9b1-4b6b-4402-80b3-b307c3ba2c8d" , PostId = Guid.NewGuid().ToString(), Title = "Fifth Post", Body = "This is the body of the fifth post.", PostedAt = DateTime.Now },
            new Post() {UserId="ef23d9b1-4b6b-4402-80b3-b307c3ba2c8d" , PostId = Guid.NewGuid().ToString(), Title = "Sixth Post", Body = "This is the body of the sixth post.", PostedAt = DateTime.Now },
            new Post() {UserId="ef23d9b1-4b6b-4402-80b3-b307c3ba2c8d" , PostId = Guid.NewGuid().ToString(), Title = "Seventh Post", Body = "This is the body of the seventh post.", PostedAt = DateTime.Now },
            new Post() {UserId="ef23d9b1-4b6b-4402-80b3-b307c3ba2c8d" , PostId = Guid.NewGuid().ToString(), Title = "Eighth Post", Body = "This is the body of the eighth post.", PostedAt = DateTime.Now },
            new Post() {UserId="ef23d9b1-4b6b-4402-80b3-b307c3ba2c8d" , PostId = Guid.NewGuid().ToString(), Title = "Ninth Post", Body = "This is the body of the ninth post.", PostedAt = DateTime.Now },
            new Post() {UserId="ef23d9b1-4b6b-4402-80b3-b307c3ba2c8d" , PostId = Guid.NewGuid().ToString(), Title = "Tenth Post", Body = "This is the body of the tenth post.", PostedAt = DateTime.Now },
            new Post() {UserId="ef23d9b1-4b6b-4402-80b3-b307c3ba2c8d" , PostId = Guid.NewGuid().ToString(), Title = "Eleventh Post", Body = "This is the body of the eleventh post.", PostedAt = DateTime.Now }
        };
    }
}
