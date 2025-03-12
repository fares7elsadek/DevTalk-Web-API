using DevTalk.Domain.Entites;
using DevTalk.Domain.Repositories;
using DevTalk.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.Data.SqlClient;

namespace DevTalk.Infrastructure.Repositories;

public class PostRepository : Repository<Post>, IPostRepository
{
    private readonly AppDbContext _db;
    public PostRepository(AppDbContext db):base(db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Post>> GetAllPostsPagination(string cursor, int pageSize, string? IncludeProperties = null)
    {
        IQueryable<Post> query = _db.Posts.AsSplitQuery();
        if (!string.IsNullOrEmpty(IncludeProperties))
        {
            foreach (var property in IncludeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(property);
            }
        }

        if(!string.IsNullOrEmpty(cursor))
            query = query.Where(x => string.Compare(x.PostId,cursor) > 0);

        return await query
            .OrderBy(x => x.PostId)
            .Take(pageSize).ToListAsync();
    }

    public async Task<IEnumerable<Post>> GetFeedPostsPagination(string idCursor, List<string> prefernceCategories, DateTime? dateTimeCursor, double? scoreCursor, int pageSize, string? IncludeProperties = null)
    {
        IQueryable<Post> query = _db.Posts.AsSplitQuery();
        if (!string.IsNullOrEmpty(IncludeProperties))
        {
            foreach (var property in IncludeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(property);
            }
        }
        
        query = query.Where(p => p.Categories.Any(c => prefernceCategories.Contains(c.CategoryId)));

        if (scoreCursor.HasValue && dateTimeCursor.HasValue && !string.IsNullOrEmpty(idCursor))
        {
            query = query.Where(p =>
                 p.PopularityScore < scoreCursor ||
                (p.PopularityScore == scoreCursor && p.PostedAt < dateTimeCursor) ||
                (p.PopularityScore == scoreCursor && p.PostedAt == dateTimeCursor && string.Compare(p.PostId, idCursor) > 0)
            );
        }

        query = query.OrderByDescending(p => p.PopularityScore)
                     .ThenByDescending(p => p.PostedAt)
                     .ThenBy(p => p.PostId)
                     .Take(pageSize);

        return await query.ToListAsync();
    }

    public async Task<bool> DeletePostWithRelation(string postId)
    {
        var postIdParam = new SqlParameter("@postId",postId);
        var statusParam = new SqlParameter
        {
            ParameterName = "@status",
            SqlDbType = System.Data.SqlDbType.Int,
            Direction = System.Data.ParameterDirection.Output
        };
        await _db.Database.ExecuteSqlRawAsync("EXEC DeletePostWithRelations @postId, @status OUTPUT", postIdParam, statusParam);
        int status = (int)statusParam.Value;
        return status == 1;
    }

    public async Task<IEnumerable<Post>> GetTrendingPostsPagination(string idCursor, DateTime? dateTimeCursor, double? scoreCursor, int pageSize, string? IncludeProperties = null)
    {
        IQueryable<Post> query = _db.Posts.AsSplitQuery();
        if (!string.IsNullOrEmpty(IncludeProperties))
        {
            foreach (var property in IncludeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(property);
            }
        }

        if (scoreCursor.HasValue && dateTimeCursor.HasValue && !string.IsNullOrEmpty(idCursor))
        {
            query = query.Where(p =>
                p.PopularityScore < scoreCursor ||  
                (p.PopularityScore == scoreCursor && p.PostedAt < dateTimeCursor) || 
                (p.PopularityScore == scoreCursor && p.PostedAt == dateTimeCursor && string.Compare(p.PostId, idCursor) > 0) 
            );
        }

        query = query.OrderByDescending(p => p.PopularityScore)
                     .ThenByDescending(p => p.PostedAt)
                     .ThenBy(p => p.PostId)
                     .Take(pageSize);

        return await query.ToListAsync();
    }
}
