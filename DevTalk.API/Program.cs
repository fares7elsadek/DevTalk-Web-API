using DevTalk.API.Extensions;
using DevTalk.API.Middlewares;
using DevTalk.Application.Extensions;
using DevTalk.Domain.Entites;
using DevTalk.Domain.Helpers;
using DevTalk.Infrastructure.Extensions;
using DevTalk.Infrastructure.Seeder;
using Microsoft.Extensions.FileProviders;
using Serilog;

namespace DevTalk.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);


        builder.Services.AddInfrastructure(builder.Configuration);
        builder.AddPresentation();
        builder.Services.AddApplication();
        builder.Services.AddEndpointsApiExplorer();
        
        var app = builder.Build();
        DevTalkSeeder(app);

        app.UseMiddleware<ErrorHandlingMiddleware>();


        if (true)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
        

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseCors();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseSerilogRequestLogging();
        app.MapControllers();

        app.Run();
    }

    public static void DevTalkSeeder(WebApplication app)
    {
        var scope = app.Services.CreateScope();
        var seeder = scope.ServiceProvider.GetRequiredService<IDevTalkSeeder>();
        seeder.Seed();
    }
}
