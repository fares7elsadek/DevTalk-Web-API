using DevTalk.API.Extensions;
using DevTalk.API.Middlewares;
using DevTalk.Application.Extensions;
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
        
        app.UseSwagger();
        app.UseSwaggerUI();
        

        app.UseHttpsRedirection();
        string uploadsPath = Path.Combine(builder.Environment.ContentRootPath, "Uploads");
        if (!Directory.Exists(uploadsPath))
        {
            Directory.CreateDirectory(uploadsPath);
        }
        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(uploadsPath),
            RequestPath = "/Resources"
        });
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
