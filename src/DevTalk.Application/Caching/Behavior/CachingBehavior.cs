using DevTalk.Application.Services.Caching;
using MediatR;

namespace DevTalk.Application.Caching.Behavior;

public class CachingBehavior<TRequest, TResponse>(ICachingService cache) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICachableRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // try to get from cache first
        var cachedResponse = await cache.GetData<TResponse>(request.Key);
        if(cachedResponse is not null)
            return cachedResponse;

        // process the request
        var response = await next();

        // cache the response 
        await cache.SetData(request.Key, response, DateTimeOffset.Now.AddMinutes(2));

        return response;
    }
}
