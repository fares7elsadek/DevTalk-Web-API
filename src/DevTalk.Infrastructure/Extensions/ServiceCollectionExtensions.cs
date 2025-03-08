using DevTalk.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DevTalk.Domain.Entites;
using DevTalk.Infrastructure.Seeder;
using DevTalk.Domain.Repositories;
using DevTalk.Infrastructure.Repositories;
using DevTalk.Domain.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;


namespace DevTalk.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructure(this IServiceCollection services,IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("cs");
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(connectionString, builder =>
            {
                builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
            });
        });
        services.AddIdentity<User, IdentityRole>(options =>
        {
            options.SignIn.RequireConfirmedEmail = true;
            options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;
        })
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();
        services.Configure<DataProtectionTokenProviderOptions>(options =>
        {
            options.TokenLifespan = TimeSpan.FromHours(3); 
        });
        services.AddScoped<IDevTalkSeeder,DevTalkSeeder>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.Configure<JwtOptions>(configuration.GetSection("JWT"));
        services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(opt =>
            {
                opt.RequireHttpsMetadata = false;
                opt.SaveToken = false;
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = configuration["JWT:Issure"],
                    ValidAudience = configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]))
                };
            })
            .AddCookie(opt =>
            {
                opt.Cookie.SameSite = SameSiteMode.None;
                opt.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            })
            .AddGoogle(options =>
            {
                options.ClientId = configuration["GoogleOauth:ClientID"]!;
                options.ClientSecret = configuration["GoogleOauth:ClientSecret"]!;
                options.CallbackPath = "/api/auth/signin-google";
            });


        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("rediscs");
        });
    }
}
