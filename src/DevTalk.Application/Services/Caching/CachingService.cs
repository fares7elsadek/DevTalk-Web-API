﻿using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace DevTalk.Application.Services.Caching;

public class CachingService(IDistributedCache cache, IConnectionMultiplexer redis) : ICachingService
{
    private readonly IDatabase _redisDb = redis.GetDatabase();
    public async Task<T?> GetData<T>(string key)
    {
        var value = await cache.GetStringAsync(key);
        if (!string.IsNullOrEmpty(value))
            return JsonConvert.DeserializeObject<T>(value,new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            });
        return default;
    }

    public async Task<bool> RemoveData(string key)
    {
        var value = await cache.GetStringAsync(key);
        if (!string.IsNullOrEmpty(value))
        {
            await cache.RemoveAsync(key);
            return true;
        }
        return false;
    }

    public async Task<bool> SetData<T>(string key, T value, DateTimeOffset expirationTime)
    {
        try
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = expirationTime
            };
            var serializedValue = JsonConvert.SerializeObject(value, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            });
            await cache.SetStringAsync(key, serializedValue, options);

            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
    
    public async Task RemoveByPattern(string pattern)
    {
        var server = redis.GetServer(redis.GetEndPoints().First());
        var keys = server.Keys(pattern: pattern).ToList();

        if (keys.Any())
        {
            await Task.WhenAll(keys.Select(x => cache.RemoveAsync(x)));
        }
    }
}
