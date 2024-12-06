using DevTalk.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DevTalk.Domain.Entites;
using DevTalk.Infrastructure.Seeder;
using DevTalk.Domain.Repositories;
using DevTalk.Infrastructure.Repositories;


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
        services.AddIdentityApiEndpoints<User>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>();
        services.AddScoped<IDevTalkSeeder,DevTalkSeeder>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}
