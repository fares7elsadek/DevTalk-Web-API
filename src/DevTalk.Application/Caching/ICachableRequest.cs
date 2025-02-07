using MediatR;

namespace DevTalk.Application.Caching;

public interface ICachableRequest<TResponse> : IRequest<TResponse>
{
    /// <summary>
    /// A unique key representing the request.
    /// </summary>
    public string Key { get; }

    /// <summary>
    /// How long the cached value should live.
    /// </summary>
    TimeSpan? CacheExpiryTime { get; }
}
