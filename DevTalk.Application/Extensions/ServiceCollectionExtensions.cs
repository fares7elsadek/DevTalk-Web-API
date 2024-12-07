using DevTalk.Application.Posts.Dtos;
using DevTalk.Application.Posts.Queries.GetAllPosts;
using DevTalk.Application.Services;
using DevTalk.Application.User;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace DevTalk.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApplication(this IServiceCollection services)
    {
        var assemply = typeof(ServiceCollectionExtensions).Assembly;
        services.AddAutoMapper(assemply);
        services.AddValidatorsFromAssembly(assemply)
            .AddFluentValidationAutoValidation();
        services.AddMediatR(cfg => 
        cfg.RegisterServicesFromAssemblies(assemply));
        services.AddHttpContextAccessor();
        services.AddScoped<IFileService, FileService>();
        services.AddScoped<IUserContext, UserContext>();
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(
            policy =>
            {
                policy.WithOrigins("*").AllowAnyMethod().AllowAnyHeader(); 
            });
        });
        services.Configure<FormOptions>(options =>
        {
            options.MultipartBodyLengthLimit = 5 * 1024 * 1024;
        });
        
    }

    public static void SeriLogConfigurations(this IHostBuilder host)
    {
        host.UseSerilog((context , loggerConfig) =>
        {
            loggerConfig.ReadFrom.Configuration(context.Configuration);
        });
    }
}
