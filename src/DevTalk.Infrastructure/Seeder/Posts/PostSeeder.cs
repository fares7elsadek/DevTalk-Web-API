using DevTalk.Domain.Entites;
using DevTalk.Domain.Constants;
using DevTalk.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace DevTalk.Infrastructure.Seeder.Posts
{
    public class PostSeeder : IPostSeeder
    {
        private readonly AppDbContext _db;
        private readonly UserManager<User> _userManager;
        private readonly string _guid = Guid.NewGuid().ToString();
        private readonly IConfiguration _configuration;

        public PostSeeder(AppDbContext db, UserManager<User> userManager,
            IConfiguration configuration)
        {
            _db = db;
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task Seed()
        {
            if (await _db.Database.CanConnectAsync())
            {
                if (!_db.Users.Any())
                {
                    var user = new User
                    {
                        FirstName = _configuration["DefaultUser:firstname"]!,
                        LastName = _configuration["DefaultUser:lastname"]!,
                        Email = _configuration["DefaultUser:email"]!,
                        UserName = _configuration["DefaultUser:username"]!,
                        AvatarUrl = _configuration["DefaultUser:avatar"],
                        AvatarFileName = _configuration["DefaultUser:filename"],
                        EmailConfirmed = true,
                        Id = _guid
                    };
                    var password = _configuration["DefaultUser:password"]!;
                    await _userManager.CreateAsync(user, password);
                }

                if (!_db.Category.Any())
                {
                    var categories = GetCategories;
                    _db.Category.AddRange(categories);
                    await _db.SaveChangesAsync();
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
            var posts = new List<Post>();

            // Post 1: AI Trends
            var post1Id = Guid.NewGuid().ToString();
            posts.Add(new Post
            {
                PostId = post1Id,
                Title = "The Future of AI: Trends and Predictions",
                Body = "Discover how artificial intelligence is transforming industries. In this article, we explore breakthroughs and predictions that are reshaping our future.",
                UpdatedAt = null,
                UserId = _guid, 
                PopularityScore = 85.0,
                PostMedias = new List<PostMedia>
                {
                    new PostMedia
                    {
                        PostMediaId = Guid.NewGuid().ToString(),
                        Type = PostMediaTypes.Image,
                        MediaUrl = "https://devtalkstorage.blob.core.windows.net/postphotos/ai_trends.jpg",
                        MediaFileName = "ai_trends.jpg",
                        PostId = post1Id
                    }
                },
                Votes = new List<PostVotes>(),
                Comments = new List<Comment>(),
                Categories = new List<Categories>(),
                PostCategories = new List<PostCategory>
                {
                    new PostCategory { PostId = post1Id, CategoryId = "cat-ai" },
                    new PostCategory { PostId = post1Id, CategoryId = "cat-ds" },
                    new PostCategory { PostId = post1Id, CategoryId = "cat-ml" },
                },
                Bookmarks = new List<Bookmarks>()
            });

            // Post 2: Cloud Computing
            var post2Id = Guid.NewGuid().ToString();
            posts.Add(new Post
            {
                PostId = post2Id,
                Title = "Understanding Cloud Computing: Benefits and Challenges",
                Body = "Cloud computing is revolutionizing business operations. This post covers the advantages and the hurdles in adopting cloud solutions.",
                UpdatedAt = null,
                UserId = _guid,
                PopularityScore = 78.0,
                PostMedias = new List<PostMedia>
                {
                    new PostMedia
                    {
                        PostMediaId = Guid.NewGuid().ToString(),
                        Type = PostMediaTypes.Image,
                        MediaUrl = "https://devtalkstorage.blob.core.windows.net/postphotos/cloud_computing.png",
                        MediaFileName = "cloud_computing.png",
                        PostId = post2Id
                    }
                },
                Votes = new List<PostVotes>(),
                Comments = new List<Comment>(),
                Categories = new List<Categories>(),
                PostCategories = new List<PostCategory>
                {
                    new PostCategory { PostId = post2Id, CategoryId = "cat-cloud" },
                    new PostCategory { PostId = post2Id, CategoryId = "cat-aws" },
                },
                Bookmarks = new List<Bookmarks>()
            });

            // Post 3: JavaScript Frameworks
            var post3Id = Guid.NewGuid().ToString();
            posts.Add(new Post
            {
                PostId = post3Id,
                Title = "Top 10 JavaScript Frameworks to Watch in 2025",
                Body = "Explore the most popular JavaScript frameworks that are shaping the future of web development in 2025.",
                UpdatedAt = null,
                UserId = _guid,
                PopularityScore = 90.0,
                PostMedias = new List<PostMedia>
                {
                    new PostMedia
                    {
                        PostMediaId = Guid.NewGuid().ToString(),
                        Type = PostMediaTypes.Image,
                        MediaUrl = "https://devtalkstorage.blob.core.windows.net/postphotos/javascript_frameworks.png",
                        MediaFileName = "javascript_frameworks.png",
                        PostId = post3Id
                    }
                },
                Votes = new List<PostVotes>(),
                Comments = new List<Comment>(),
                Categories = new List<Categories>(),
                PostCategories = new List<PostCategory>
                {
                    new PostCategory { PostId = post3Id, CategoryId = "cat-js" }
                },
                Bookmarks = new List<Bookmarks>()
            });

            // Post 4: Python for Data Science
            var post4Id = Guid.NewGuid().ToString();
            posts.Add(new Post
            {
                PostId = post4Id,
                Title = "Getting Started with Python for Data Science",
                Body = "Learn how Python is used in data science and analytics. This post introduces key libraries and techniques for beginners.",
                UpdatedAt = null,
                UserId = _guid,
                PopularityScore = 88.0,
                PostMedias = new List<PostMedia>
                {
                    new PostMedia
                    {
                        PostMediaId = Guid.NewGuid().ToString(),
                        Type = PostMediaTypes.Image,
                        MediaUrl = "https://devtalkstorage.blob.core.windows.net/postphotos/python_data_science.jpg",
                        MediaFileName = "python_data_science.jpg",
                        PostId = post4Id
                    }
                },
                Votes = new List<PostVotes>(),
                Comments = new List<Comment>(),
                Categories = new List<Categories>(),
                PostCategories = new List<PostCategory>
                {
                    new PostCategory { PostId = post4Id, CategoryId = "cat-python" },
                    new PostCategory { PostId = post4Id, CategoryId = "cat-ds" },
                    new PostCategory { PostId = post4Id, CategoryId = "cat-ai" },
                },
                Bookmarks = new List<Bookmarks>()
            });

            // Post 5: DevOps Best Practices
            var post5Id = Guid.NewGuid().ToString();
            posts.Add(new Post
            {
                PostId = post5Id,
                Title = "DevOps Best Practices: Tools and Techniques",
                Body = "Dive into the best practices in DevOps. Learn about the essential tools and methodologies to streamline your development process.",
                UpdatedAt = null,
                UserId = _guid,
                PopularityScore = 82.0,
                PostMedias = new List<PostMedia>
                {
                    new PostMedia
                    {
                        PostMediaId = Guid.NewGuid().ToString(),
                        Type = PostMediaTypes.Image,
                        MediaUrl = "https://devtalkstorage.blob.core.windows.net/postphotos/devops_best_practices.jpg",
                        MediaFileName = "devops_best_practices.jpg",
                        PostId = post5Id
                    }
                },
                Votes = new List<PostVotes>(),
                Comments = new List<Comment>(),
                Categories = new List<Categories>(),
                PostCategories = new List<PostCategory>
                {
                    new PostCategory { PostId = post5Id, CategoryId = "cat-devops" },
                    new PostCategory { PostId = post5Id, CategoryId = "cat-aws" },
                    new PostCategory { PostId = post5Id, CategoryId = "cat-azure" },
                    new PostCategory { PostId = post5Id, CategoryId = "cat-cloud" },
                },
                Bookmarks = new List<Bookmarks>()
            });

            return posts;
        }

        public IEnumerable<Categories> GetCategories =>
            new List<Categories>()
            {
                new Categories { CategoryId = "cat-js", CategoryName = "JavaScript" },
                new Categories { CategoryId = "cat-ts", CategoryName = "TypeScript" },
                new Categories { CategoryId = "cat-react", CategoryName = "React" },
                new Categories { CategoryId = "cat-angular", CategoryName = "Angular" },
                new Categories { CategoryId = "cat-vue", CategoryName = "Vue" },
                new Categories { CategoryId = "cat-node", CategoryName = "Node.js" },
                new Categories { CategoryId = "cat-python", CategoryName = "Python" },
                new Categories { CategoryId = "cat-django", CategoryName = "Django" },
                new Categories { CategoryId = "cat-flask", CategoryName = "Flask" },
                new Categories { CategoryId = "cat-ml", CategoryName = "Machine Learning" },
                new Categories { CategoryId = "cat-ai", CategoryName = "Artificial Intelligence" },
                new Categories { CategoryId = "cat-ds", CategoryName = "Data Science" },
                new Categories { CategoryId = "cat-devops", CategoryName = "DevOps" },
                new Categories { CategoryId = "cat-kubernetes", CategoryName = "Kubernetes" },
                new Categories { CategoryId = "cat-docker", CategoryName = "Docker" },
                new Categories { CategoryId = "cat-cloud", CategoryName = "Cloud Computing" },
                new Categories { CategoryId = "cat-aws", CategoryName = "AWS" },
                new Categories { CategoryId = "cat-azure", CategoryName = "Azure" },
                new Categories { CategoryId = "cat-google", CategoryName = "Google Cloud" },
                new Categories { CategoryId = "cat-cyber", CategoryName = "Cybersecurity" },
                new Categories { CategoryId = "cat-webdev", CategoryName = "Web Development" },
                new Categories { CategoryId = "cat-mobile", CategoryName = "Mobile Development" },
                new Categories { CategoryId = "cat-ios", CategoryName = "iOS" },
                new Categories { CategoryId = "cat-android", CategoryName = "Android" },
                new Categories { CategoryId = "cat-csharp", CategoryName = "C#" },
                new Categories { CategoryId = "cat-dotnet", CategoryName = ".NET" },
                new Categories { CategoryId = "cat-java", CategoryName = "Java" },
                new Categories { CategoryId = "cat-php", CategoryName = "PHP" },
                new Categories { CategoryId = "cat-rust", CategoryName = "Rust" },
                new Categories { CategoryId = "cat-go", CategoryName = "Go" },
                new Categories { CategoryId = "cat-blockchain", CategoryName = "Blockchain" },
                new Categories { CategoryId = "cat-crypto", CategoryName = "Cryptocurrency" },
                new Categories { CategoryId = "cat-uiux", CategoryName = "UI/UX Design" },
                new Categories { CategoryId = "cat-arch", CategoryName = "Software Architecture" },
                new Categories { CategoryId = "cat-agile", CategoryName = "Agile" },
                new Categories { CategoryId = "cat-testing", CategoryName = "Testing" },
                new Categories { CategoryId = "cat-performance", CategoryName = "Performance" }
            };
    }
}
