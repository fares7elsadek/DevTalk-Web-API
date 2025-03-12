namespace DevTalk.Application.Services.Caching;

public interface ICachingService
{
    Task<T> GetData<T>(string key);
    Task<bool> SetData<T>(string key, T value, DateTimeOffset expirationTime);
    Task<bool> RemoveData(string key);
    Task RemoveByPattern(string pattern);
}
