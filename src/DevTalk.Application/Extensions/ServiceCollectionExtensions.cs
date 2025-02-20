using DevTalk.Application.Posts.Dtos;
using DevTalk.Application.Posts.Queries.GetAllPosts;
using DevTalk.Application.Services;
using DevTalk.Application.ApplicationUser;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Serilog;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity.UI.Services;
using DevTalk.Application.Services.EmailService;
using DevTalk.Domain.Entites;
using Microsoft.AspNetCore.Identity;
using DevTalk.Domain.Helpers;
using Humanizer.Configuration;
using DevTalk.Application.Services.Caching;
using MediatR;
using DevTalk.Application.Caching;
using DevTalk.Application.Caching.Behavior;
using StackExchange.Redis;
using MassTransit;
using DevTalk.Application.Notification.MessageQueue;
using DevTalk.Application.Services.Notification;

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
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IEmailSender<User>,EmailSender>();
        services.AddScoped<ICachingService,CachingService>();
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>));
        services.AddMassTransit(x =>
        {
            x.AddConsumer<NotificationConsumer>();

            x.UsingInMemory((context, cfg) =>
            {
                cfg.ConfigureEndpoints(context);
            });
        });
        services.AddScoped<INotificationService,NotificationService>();
    }

    public static void SeriLogConfigurations(this IHostBuilder host)
    {
        host.UseSerilog((context , loggerConfig) =>
        {
            loggerConfig.ReadFrom.Configuration(context.Configuration);
        });
    }
}
