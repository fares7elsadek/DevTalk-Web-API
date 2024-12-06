using DevTalk.API.Middlewares;

namespace DevTalk.API.Extensions;

public static class  ServiceCollectionExtensions
{
    public static void AddPresentation(this IServiceCollection services)
    {
        services.AddScoped<ErrorHandlingMiddleware>();
    }
}
