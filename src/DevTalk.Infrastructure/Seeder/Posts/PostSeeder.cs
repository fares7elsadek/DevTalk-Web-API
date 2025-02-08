using DevTalk.Domain.Entites;
using DevTalk.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;

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

            if (!_db.Category.Any())
            {
                var categories = GetCategories;
                _db.Category.AddRange(categories);
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

    public IEnumerable<Categories> GetCategories =>
        new List<Categories>()
        {
            new Categories { CategoryId = Guid.NewGuid().ToString(), CategoryName = "JavaScript" },
            new Categories { CategoryId = Guid.NewGuid().ToString(), CategoryName = "TypeScript" },
            new Categories { CategoryId = Guid.NewGuid().ToString(), CategoryName = "React" },
            new Categories { CategoryId = Guid.NewGuid().ToString(), CategoryName = "Angular" },
            new Categories { CategoryId = Guid.NewGuid().ToString(), CategoryName = "Vue" },
            new Categories { CategoryId = Guid.NewGuid().ToString(), CategoryName = "Node.js" },
            new Categories { CategoryId = Guid.NewGuid().ToString(), CategoryName = "Python" },
            new Categories { CategoryId = Guid.NewGuid().ToString(), CategoryName = "Django" },
            new Categories { CategoryId = Guid.NewGuid().ToString(), CategoryName = "Flask" },
            new Categories { CategoryId = Guid.NewGuid().ToString(), CategoryName = "Machine Learning" },
            new Categories { CategoryId = Guid.NewGuid().ToString(), CategoryName = "Artificial Intelligence" },
            new Categories { CategoryId = Guid.NewGuid().ToString(), CategoryName = "Data Science" },
            new Categories { CategoryId = Guid.NewGuid().ToString(), CategoryName = "DevOps" },
            new Categories { CategoryId = Guid.NewGuid().ToString(), CategoryName = "Kubernetes" },
            new Categories { CategoryId = Guid.NewGuid().ToString(), CategoryName = "Docker" },
            new Categories { CategoryId = Guid.NewGuid().ToString(), CategoryName = "Cloud Computing" },
            new Categories { CategoryId = Guid.NewGuid().ToString(), CategoryName = "AWS" },
            new Categories { CategoryId = Guid.NewGuid().ToString(), CategoryName = "Azure" },
            new Categories { CategoryId = Guid.NewGuid().ToString(), CategoryName = "Google Cloud" },
            new Categories { CategoryId = Guid.NewGuid().ToString(), CategoryName = "Cybersecurity" },
            new Categories { CategoryId = Guid.NewGuid().ToString(), CategoryName = "Web Development" },
            new Categories { CategoryId = Guid.NewGuid().ToString(), CategoryName = "Mobile Development" },
            new Categories { CategoryId = Guid.NewGuid().ToString(), CategoryName = "iOS" },
            new Categories { CategoryId = Guid.NewGuid().ToString(), CategoryName = "Android" },
            new Categories { CategoryId = Guid.NewGuid().ToString(), CategoryName = "C#" },
            new Categories { CategoryId = Guid.NewGuid().ToString(), CategoryName = ".NET" },
            new Categories { CategoryId = Guid.NewGuid().ToString(), CategoryName = "Java" },
            new Categories { CategoryId = Guid.NewGuid().ToString(), CategoryName = "PHP" },
            new Categories { CategoryId = Guid.NewGuid().ToString(), CategoryName = "Rust" },
            new Categories { CategoryId = Guid.NewGuid().ToString(), CategoryName = "Go" },
            new Categories { CategoryId = Guid.NewGuid().ToString(), CategoryName = "Blockchain" },
            new Categories { CategoryId = Guid.NewGuid().ToString(), CategoryName = "Cryptocurrency" },
            new Categories { CategoryId = Guid.NewGuid().ToString(), CategoryName = "UI/UX Design" },
            new Categories { CategoryId = Guid.NewGuid().ToString(), CategoryName = "Software Architecture" },
            new Categories { CategoryId = Guid.NewGuid().ToString(), CategoryName = "Agile" },
            new Categories { CategoryId = Guid.NewGuid().ToString(), CategoryName = "Testing" },
            new Categories { CategoryId = Guid.NewGuid().ToString(), CategoryName = "Performance" }
        };
}
