using Azure;
using DevTalk.Domain.Entites;
using DevTalk.Domain.Repositories;
using DevTalk.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Linq.Expressions;

namespace DevTalk.Infrastructure.Repositories;

public class Repository<T> : IRepositories<T> where T : class
{
    private readonly AppDbContext _db;
    private readonly DbSet<T> _dbSet;
    public Repository(AppDbContext db)
    {
        _db = db;
        _dbSet = db.Set<T>();
    }
    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public async Task<IEnumerable<T>> GetAllAsync(string? IncludeProperties = null)
    {
        IQueryable<T> query = this._dbSet;
        if (!string.IsNullOrEmpty(IncludeProperties))
        {
            foreach (var property in IncludeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(property);
            }
        }
        return await query.AsSplitQuery().ToListAsync();
    }

    public async Task<IEnumerable<T>> GetAllWithConditionAsync(Expression<Func<T, bool>> filter, string? IncludeProperties = null)
    {
        IQueryable<T> query = this._dbSet;
        if (!string.IsNullOrEmpty(IncludeProperties))
        {
            foreach (var property in IncludeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(property);
            }
        }
        return await query.AsSplitQuery().Where(filter).ToListAsync();
    }

    public async Task<IEnumerable<T>> GetAllWithPagination(Expression<Func<T, bool>> filter, int page, int size, string? IncludeProperties = null)
    {
        IQueryable<T> query = this._dbSet.AsSplitQuery();
        int total = query.Count();
        if (total == 0)
            return [];

        if (!string.IsNullOrEmpty(IncludeProperties))
        {
            foreach (var property in IncludeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(property);
            }
        }

        if (page < 0) page = 1;
        if (size > total) size = 5;
        int pages = (int)Math.Ceiling((decimal)total / size);
        if (page > pages)
        {
            page = pages;
        }
        
        return await query.Where(filter).Skip((page - 1) * size)
            .Take(size).ToListAsync();
    }

    public async Task<T?> GetOrDefalutAsync(Expression<Func<T, bool>> filter, string? IncludeProperties = null)
    {
        IQueryable<T> query = _dbSet;

        if (!string.IsNullOrWhiteSpace(IncludeProperties))
        {
            foreach (var property in IncludeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(property.Trim());
            }
        }

        return await query.AsSplitQuery().FirstOrDefaultAsync(filter);
    }

    public void Remove(T entity)
    {
        _dbSet.Remove(entity);
    }

    public void RemoveRange(IEnumerable<T> entities)
    {
        _dbSet.RemoveRange(entities);
    }

    public void Update(T entity)
    {
        _db.Update(entity);
    }
}

