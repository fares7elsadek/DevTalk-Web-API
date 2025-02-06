using DevTalk.Domain.Repositories;
using DevTalk.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;

namespace DevTalk.Infrastructure.Repositories;

public class CachedRepository<T> : IRepositories<T> where T : class
{
    private readonly IRepositories<T> _innerRepository;
    private readonly IDistributedCache _cache;
    private readonly string _cacheKeyPrefix;
    private readonly DistributedCacheEntryOptions _cacheOptions;
    private readonly AppDbContext _db;
    private readonly DbSet<T> _dbSet;

    public CachedRepository(IRepositories<T> innerRepository, IDistributedCache cache, string cacheKeyPrefix,
        AppDbContext db)
    {
        _innerRepository = innerRepository;
        _cache = cache;
        _cacheKeyPrefix = cacheKeyPrefix;
        _cacheOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
        };
        _db = db;
        _dbSet = db.Set<T>();
    }
    public async Task AddAsync(T entity)
    {
        await _innerRepository.AddAsync(entity);
        await _cache.RemoveAsync($"{_cacheKeyPrefix}:all");
    }

    public async Task<IEnumerable<T>> GetAllAsync(string? IncludeProperties = null)
    {
        string includeKeyPart = IncludeProperties != null ? $":{IncludeProperties}" : string.Empty;
        string cacheKey = $"{_cacheKeyPrefix}:all{includeKeyPart}";

        var cachedData = await _cache.GetStringAsync(cacheKey);
        if (!string.IsNullOrEmpty(cachedData))
        {
            var data = JsonConvert.DeserializeObject<IEnumerable<T>>(cachedData);
            return data;
        }

        var entites = await _innerRepository.GetAllAsync(IncludeProperties);
        await _cache.SetStringAsync(cacheKey,JsonConvert.SerializeObject(entites),
            _cacheOptions);
        return entites;
    }

    public async Task<IEnumerable<T>> GetAllWithConditionAsync(Expression<Func<T, bool>> filter, string? IncludeProperties = null)
    {
        string cacheKey = GenerateCacheKey(filter, IncludeProperties);

        var cachedData = await _cache.GetStringAsync(cacheKey);
        if (!string.IsNullOrEmpty(cachedData))
        {
            var data = JsonConvert.DeserializeObject<IEnumerable<T>>(cachedData);
            return data;
        }

        var entities = await _innerRepository.GetAllWithConditionAsync(filter, IncludeProperties);
        await _cache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(entities), _cacheOptions);
        return entities;
    }

    public async Task<T?> GetOrDefalutAsync(Expression<Func<T, bool>> filter, string? IncludeProperties = null)
    {
        string cacheKey = GenerateCacheKey(filter, IncludeProperties);

        var cachedData = await _cache.GetStringAsync(cacheKey);
        if (!string.IsNullOrEmpty(cachedData))
        {
            var data = JsonConvert.DeserializeObject<T>(cachedData);
            if ( data is not null)
                _dbSet.Attach(data);
            return data;
        }

        var entity = await _innerRepository.GetOrDefalutAsync(filter, IncludeProperties);
        await _cache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(entity), _cacheOptions);
        return entity;
    }

    public void Remove(T entity)
    {
        _innerRepository.Remove(entity);
        _ = InvalidateCacheAsync("all");
    }

    public void RemoveRange(IEnumerable<T> entities)
    {
        _innerRepository.RemoveRange(entities);
        _ = InvalidateCacheAsync("all");
    }

    private async Task InvalidateCacheAsync(string keySuffix)
    {
        string cacheKeyPattern = $"{_cacheKeyPrefix}:{keySuffix}";
        await _cache.RemoveAsync(cacheKeyPattern);
    }

    private string GenerateCacheKey(Expression<Func<T, bool>> filter, string? includeProperties)
    {
        string filterString = filter.ToString();
        string filterHash = ComputeHash(filterString);
        string includeKey = includeProperties ?? string.Empty;
        return $"{_cacheKeyPrefix}:filter:{filterHash}:{includeKey}";
    }

    private string ComputeHash(string input)
    {
        using (var sha256 = SHA256.Create())
        {
            byte[] bytes = Encoding.UTF8.GetBytes(input);
            byte[] hashBytes = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hashBytes);
        }
    }

    public void Update(T entity)
    {
        _innerRepository.Update(entity);
        _ = InvalidateCacheAsync("all");
    }
}
