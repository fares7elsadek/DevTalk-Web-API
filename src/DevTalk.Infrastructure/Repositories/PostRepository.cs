using DevTalk.Domain.Entites;
using DevTalk.Domain.Repositories;
using DevTalk.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using AutoMapper;
using Dapper;
using DevTalk.Domain.Abstractions;
using Microsoft.Data.SqlClient;

namespace DevTalk.Infrastructure.Repositories;

public class PostRepository : Repository<Post>, IPostRepository
{
    private readonly AppDbContext _db;
    private readonly ISqlConnectionFactory _dapper;
    public PostRepository(AppDbContext db,ISqlConnectionFactory dapper):base(db)
    {
        _db = db;
        _dapper = dapper;
    }

    public async Task<IEnumerable<Post>> GetAllPostsPagination(string cursor, int pageSize)
    {
        var sql = @"
                -- Structure : Temp for pagination
                CREATE TEMP TABLE TempPaginatedPosts AS
                SELECT * FROM ""Posts""
                WHERE (@Cursor = '' OR ""PostId"" > @Cursor)
                ORDER BY ""PostId""
                LIMIT @PageSize;
                
                -- First result set: posts with pagination
                SELECT * FROM TempPaginatedPosts;
                
                -- Second result set: post medias for the retrieved posts
                SELECT pm.* FROM ""PostMedia"" pm
                INNER JOIN TempPaginatedPosts p ON p.""PostId"" = pm.""PostId"";
                
                -- Third result set: Votes for the retrieved posts
                SELECT v.* FROM ""PostVotes"" v
                INNER JOIN TempPaginatedPosts p ON p.""PostId"" = v.""PostId"";

                -- Fourth result set: Comments for the retrieved posts
                SELECT c.* FROM ""Comments"" c
                INNER JOIN TempPaginatedPosts p ON p.""PostId"" = c.""PostId"";

                -- Fifth result set: User for the retrieved posts
                SELECT u.* FROM ""Users"" u
                INNER JOIN TempPaginatedPosts p ON u.""Id"" = p.""UserId"";
                
                -- Sixth result set: Categories for the retrieved posts
                SELECT * FROM ""Category"";

                SELECT pc.* FROM ""PostCategory"" pc
                INNER JOIN TempPaginatedPosts p ON pc.""PostId"" = p.""PostId"";
                
                -- Drop the temporary table to free up memory
                DROP TABLE TempPaginatedPosts;
                ";

        
        using IDbConnection connection = _dapper.CreateConnection();
        var multi = await connection.QueryMultipleAsync(sql, new { Cursor = cursor, PageSize = pageSize });
        
        var posts = (await multi.ReadAsync<Post>()).ToList();          // Result set 1: Posts
        var medias = (await multi.ReadAsync<PostMedia>()).ToList();      // Result set 2: PostMedia
        var votes = (await multi.ReadAsync<PostVotes>()).ToList();       // Result set 3: PostVotes
        var comments = (await multi.ReadAsync<Comment>()).ToList();      // Result set 4: Comments
        var users = (await multi.ReadAsync<User>()).ToList();            // Result set 5: Users
        var categories = (await multi.ReadAsync<Categories>()).ToList();   // Result set 6: Category
        var postCategories = (await multi.ReadAsync<PostCategory>()).ToList();  // Result set 7: PostCategory

        foreach (var post in posts)
        {
            post.PostMedias = medias.Where(pm => pm.PostId == post.PostId).ToList();
            post.Comments = comments.Where(c => c.PostId == post.PostId).ToList();
            post.User = users.First(x => x.Id == post.UserId);
            post.Votes = votes.Where(x => x.PostId == post.PostId).ToList();
            var categoriesOfPost = postCategories.Where(x => x.PostId == post.PostId).Select(x => x.CategoryId).ToList();
            post.Categories = categories.Where(x => categoriesOfPost.Contains(x.CategoryId)).ToList();
        }
        
        return posts;
    }

    public async Task<IEnumerable<Post>> GetFeedPostsPagination(string idCursor, string userId, DateTime? dateTimeCursor, double? scoreCursor, int pageSize)
    {
        var sql = @"
                -- Structure : Temp for preferences
                CREATE TEMP TABLE TempPreferencesCategory AS
                SELECT * FROM  ""Preferences""
                WHERE ""UserId"" = @userId;

                -- Structure : Temp for pagination
                CREATE TEMP TABLE TempPaginatedPosts AS
                SELECT DISTINCT p.*
                FROM ""Posts"" p
                INNER JOIN ""PostCategory"" pc on p.""PostId"" = pc.""PostId""
                WHERE pc.""CategoryId"" IN (SELECT ""CategoryId"" FROM TempPreferencesCategory)
                  AND (
                   (@scoreCursor IS NULL OR @dateTimeCursor IS NULL OR @idCursor = '') OR
                       (""PopularityScore"" < @scoreCursor
                    OR (""PopularityScore"" = @scoreCursor AND ""PostedAt"" < @dateTimeCursor)
                    OR (""PopularityScore"" = @scoreCursor AND ""PostedAt"" = @dateTimeCursor AND p.""PostId"" > @idCursor))
                  )
                ORDER BY ""PopularityScore"", ""PostedAt"", p.""PostId""
                LIMIT @PageSize;
                
                -- First result set: posts with pagination
                SELECT * FROM TempPaginatedPosts;
                
                -- Second result set: post medias for the retrieved posts
                SELECT pm.* FROM ""PostMedia"" pm
                INNER JOIN TempPaginatedPosts p ON p.""PostId"" = pm.""PostId"";
                
                -- Third result set: Votes for the retrieved posts
                SELECT v.* FROM ""PostVotes"" v
                INNER JOIN TempPaginatedPosts p ON p.""PostId"" = v.""PostId"";

                -- Fourth result set: Comments for the retrieved posts
                SELECT c.* FROM ""Comments"" c
                INNER JOIN TempPaginatedPosts p ON p.""PostId"" = c.""PostId"";

                -- Fifth result set: User for the retrieved posts
                SELECT u.* FROM ""Users"" u
                INNER JOIN TempPaginatedPosts p ON u.""Id"" = p.""UserId"";
                
                -- Sixth result set: Categories for the retrieved posts
                SELECT * FROM ""Category"";

                SELECT pc.* FROM ""PostCategory"" pc
                INNER JOIN TempPaginatedPosts p ON pc.""PostId"" = p.""PostId"";
                
                -- Drop the temporary table to free up memory
                DROP TABLE TempPaginatedPosts;
                DROP TABLE TempPreferencesCategory;
                ";
        
        using IDbConnection connection = _dapper.CreateConnection();
        var multi = await connection.QueryMultipleAsync(sql, new { idCursor , dateTimeCursor , userId , scoreCursor ,PageSize = pageSize });
        
        var posts = (await multi.ReadAsync<Post>()).ToList();          // Result set 1: Posts
        var medias = (await multi.ReadAsync<PostMedia>()).ToList();      // Result set 2: PostMedia
        var votes = (await multi.ReadAsync<PostVotes>()).ToList();       // Result set 3: PostVotes
        var comments = (await multi.ReadAsync<Comment>()).ToList();      // Result set 4: Comments
        var users = (await multi.ReadAsync<User>()).ToList();            // Result set 5: Users
        var categories = (await multi.ReadAsync<Categories>()).ToList();   // Result set 6: Category
        var postCategories = (await multi.ReadAsync<PostCategory>()).ToList();  // Result set 7: PostCategory

        foreach (var post in posts)
        {
            post.PostMedias = medias.Where(pm => pm.PostId == post.PostId).ToList();
            post.Comments = comments.Where(c => c.PostId == post.PostId).ToList();
            post.User = users.First(x => x.Id == post.UserId);
            post.Votes = votes.Where(x => x.PostId == post.PostId).ToList();
            var categoriesOfPost = postCategories.Where(x => x.PostId == post.PostId).Select(x => x.CategoryId).ToList();
            post.Categories = categories.Where(x => categoriesOfPost.Contains(x.CategoryId)).ToList();
        }
        
        return posts;
    }

    public async Task<IEnumerable<Post>> GetPostsByCategory(string idCursor, string categoryId, DateTime? dateTimeCursor, double? scoreCursor,
        int pageSize)
    {
        var sql = @"
                -- Structure : Temp for pagination
                CREATE TEMP TABLE TempPaginatedPosts AS
                SELECT DISTINCT p.*
                FROM ""Posts"" p
                INNER JOIN ""PostCategory"" pc on p.""PostId"" = pc.""PostId""
                WHERE pc.""CategoryId"" = @categoryId
                  AND (
                   (@scoreCursor IS NULL OR @dateTimeCursor IS NULL OR @idCursor = '') OR
                       (""PopularityScore"" < @scoreCursor
                    OR (""PopularityScore"" = @scoreCursor AND ""PostedAt"" < @dateTimeCursor)
                    OR (""PopularityScore"" = @scoreCursor AND ""PostedAt"" = @dateTimeCursor AND p.""PostId"" > @idCursor))
                  )
                ORDER BY ""PopularityScore"", ""PostedAt"", p.""PostId""
                LIMIT @PageSize;
                
                -- First result set: posts with pagination
                SELECT * FROM TempPaginatedPosts;
                
                -- Second result set: post medias for the retrieved posts
                SELECT pm.* FROM ""PostMedia"" pm
                INNER JOIN TempPaginatedPosts p ON p.""PostId"" = pm.""PostId"";
                
                -- Third result set: Votes for the retrieved posts
                SELECT v.* FROM ""PostVotes"" v
                INNER JOIN TempPaginatedPosts p ON p.""PostId"" = v.""PostId"";

                -- Fourth result set: Comments for the retrieved posts
                SELECT c.* FROM ""Comments"" c
                INNER JOIN TempPaginatedPosts p ON p.""PostId"" = c.""PostId"";

                -- Fifth result set: User for the retrieved posts
                SELECT u.* FROM ""Users"" u
                INNER JOIN TempPaginatedPosts p ON u.""Id"" = p.""UserId"";
                
                -- Sixth result set: Categories for the retrieved posts
                SELECT * FROM ""Category"";

                SELECT pc.* FROM ""PostCategory"" pc
                INNER JOIN TempPaginatedPosts p ON pc.""PostId"" = p.""PostId"";
                
                -- Drop the temporary table to free up memory
                DROP TABLE TempPaginatedPosts;
                ";
        
        using IDbConnection connection = _dapper.CreateConnection();
        var multi = await connection.QueryMultipleAsync(sql, new { idCursor , dateTimeCursor , categoryId , scoreCursor ,PageSize = pageSize });
        
        var posts = (await multi.ReadAsync<Post>()).ToList();          // Result set 1: Posts
        var medias = (await multi.ReadAsync<PostMedia>()).ToList();      // Result set 2: PostMedia
        var votes = (await multi.ReadAsync<PostVotes>()).ToList();       // Result set 3: PostVotes
        var comments = (await multi.ReadAsync<Comment>()).ToList();      // Result set 4: Comments
        var users = (await multi.ReadAsync<User>()).ToList();            // Result set 5: Users
        var categories = (await multi.ReadAsync<Categories>()).ToList();   // Result set 6: Category
        var postCategories = (await multi.ReadAsync<PostCategory>()).ToList();  // Result set 7: PostCategory

        foreach (var post in posts)
        {
            post.PostMedias = medias.Where(pm => pm.PostId == post.PostId).ToList();
            post.Comments = comments.Where(c => c.PostId == post.PostId).ToList();
            post.User = users.First(x => x.Id == post.UserId);
            post.Votes = votes.Where(x => x.PostId == post.PostId).ToList();
            var categoriesOfPost = postCategories.Where(x => x.PostId == post.PostId).Select(x => x.CategoryId).ToList();
            post.Categories = categories.Where(x => categoriesOfPost.Contains(x.CategoryId)).ToList();
        }
        
        return posts;
    }

    public async Task<Post> GetPostById(string id)
    {
        var sql = @"
                -- POST Temp Table
                CREATE TEMP TABLE TempPostTable AS
                SELECT * FROM ""Posts"" WHERE ""PostId"" = @id;
                
                -- Post
                SELECT * FROM TempPostTable;
                
                -- Post medias
                SELECT * FROM ""PostMedia"" WHERE ""PostId"" = @id;

                -- Votes
                SELECT * FROM ""PostVotes"" WHERE ""PostId"" = @id;

                -- Comments
                SELECT * FROM ""Comments"" WHERE ""PostId"" = @id;

                -- User
                SELECT u.* FROM ""Users"" u INNER JOIN TempPostTable t ON u.""Id"" = t.""UserId"";
                
                -- Category
                SELECT * FROM ""Category"";
                
                -- Post Category
                SELECT * FROM ""PostCategory"" WHERE ""PostId"" = @id;
            ";
        
        using IDbConnection connection = _dapper.CreateConnection();
        var multi = await connection.QueryMultipleAsync(sql, new { id });
        
        var post = await multi.ReadFirstAsync<Post>();          // Result set 1: Post
        var medias = (await multi.ReadAsync<PostMedia>()).ToList();      // Result set 2: PostMedia
        var votes = (await multi.ReadAsync<PostVotes>()).ToList();       // Result set 3: PostVotes
        var comments = (await multi.ReadAsync<Comment>()).ToList();      // Result set 4: Comments
        var user = await multi.ReadFirstAsync<User>();            // Result set 5: User
        var categories = (await multi.ReadAsync<Categories>()).ToList();   // Result set 6: Category
        var postCategories = (await multi.ReadAsync<PostCategory>()).ToList();  // Result set 7: PostCategory

        post.PostMedias = medias;
        post.Comments = comments;
        post.User = user;
        post.Votes = votes;
        var categoriesOfPost = postCategories.Select(x => x.CategoryId).ToList();
        post.Categories = categories.Where(x => categoriesOfPost.Contains(x.CategoryId)).ToList();
        return post;
    }

    public async Task<IEnumerable<Post>> GetTrendingPostsPagination(string idCursor, DateTime? dateTimeCursor, double? scoreCursor, int pageSize)
    {
        var sql = @"
                -- Structure : Temp for pagination
                CREATE TEMP TABLE TempPaginatedPosts AS
                SELECT *
                FROM ""Posts"" 
                WHERE 
                  (
                   (@scoreCursor IS NULL OR @dateTimeCursor IS NULL OR @idCursor = '') OR
                       (""PopularityScore"" < @scoreCursor
                    OR (""PopularityScore"" = @scoreCursor AND ""PostedAt"" < @dateTimeCursor)
                    OR (""PopularityScore"" = @scoreCursor AND ""PostedAt"" = @dateTimeCursor AND ""PostId"" > @idCursor))
                  )
                ORDER BY ""PopularityScore"", ""PostedAt"", ""PostId""
                LIMIT @PageSize;
                
                -- First result set: posts with pagination
                SELECT * FROM TempPaginatedPosts;
                
                -- Second result set: post medias for the retrieved posts
                SELECT pm.* FROM ""PostMedia"" pm
                INNER JOIN TempPaginatedPosts p ON p.""PostId"" = pm.""PostId"";
                
                -- Third result set: Votes for the retrieved posts
                SELECT v.* FROM ""PostVotes"" v
                INNER JOIN TempPaginatedPosts p ON p.""PostId"" = v.""PostId"";

                -- Fourth result set: Comments for the retrieved posts
                SELECT c.* FROM ""Comments"" c
                INNER JOIN TempPaginatedPosts p ON p.""PostId"" = c.""PostId"";

                -- Fifth result set: User for the retrieved posts
                SELECT u.* FROM ""Users"" u
                INNER JOIN TempPaginatedPosts p ON u.""Id"" = p.""UserId"";
                
                -- Sixth result set: Categories for the retrieved posts
                SELECT * FROM ""Category"";

                SELECT pc.* FROM ""PostCategory"" pc
                INNER JOIN TempPaginatedPosts p ON pc.""PostId"" = p.""PostId"";
                
                -- Drop the temporary table to free up memory
                DROP TABLE TempPaginatedPosts;
                ";
        
        using IDbConnection connection = _dapper.CreateConnection();
        var multi = await connection.QueryMultipleAsync(sql, new { idCursor , dateTimeCursor  , scoreCursor ,PageSize = pageSize });
        
        var posts = (await multi.ReadAsync<Post>()).ToList();          // Result set 1: Posts
        var medias = (await multi.ReadAsync<PostMedia>()).ToList();      // Result set 2: PostMedia
        var votes = (await multi.ReadAsync<PostVotes>()).ToList();       // Result set 3: PostVotes
        var comments = (await multi.ReadAsync<Comment>()).ToList();      // Result set 4: Comments
        var users = (await multi.ReadAsync<User>()).ToList();            // Result set 5: Users
        var categories = (await multi.ReadAsync<Categories>()).ToList();   // Result set 6: Category
        var postCategories = (await multi.ReadAsync<PostCategory>()).ToList();  // Result set 7: PostCategory

        foreach (var post in posts)
        {
            post.PostMedias = medias.Where(pm => pm.PostId == post.PostId).ToList();
            post.Comments = comments.Where(c => c.PostId == post.PostId).ToList();
            post.User = users.First(x => x.Id == post.UserId);
            post.Votes = votes.Where(x => x.PostId == post.PostId).ToList();
            var categoriesOfPost = postCategories.Where(x => x.PostId == post.PostId).Select(x => x.CategoryId).ToList();
            post.Categories = categories.Where(x => categoriesOfPost.Contains(x.CategoryId)).ToList();
        }
        
        return posts;
    }
}
