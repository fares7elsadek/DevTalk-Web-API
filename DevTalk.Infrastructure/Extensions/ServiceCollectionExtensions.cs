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


namespace DevTalk.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructure(this IServiceCollection services,IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("cs");
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });
        services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>();
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
                    ValidAudience= configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]))
                };
            });
    }
}
