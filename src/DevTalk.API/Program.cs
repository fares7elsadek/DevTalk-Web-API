using DevTalk.API.Extensions;
using DevTalk.API.Hubs;
using DevTalk.API.Middlewares;
using DevTalk.Application.Extensions;
using DevTalk.Domain.Extensions;
using DevTalk.Infrastructure.Extensions;
using DevTalk.Infrastructure.Seeder;
using Serilog;

namespace DevTalk.API;

public class Program
{
    public static async Task Main(string[] args)
    {
        try
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddSignalR();
            builder.Services.AddInfrastructure(builder.Configuration);
            builder.AddPresentation();
            builder.Services.AddApplication();
            builder.Services.AddDomain(builder.Configuration);
            builder.Services.AddEndpointsApiExplorer();

            var app = builder.Build();
            await DevTalkSeeder(app);

            app.UseMiddleware<ErrorHandlingMiddleware>();


            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }



            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCors("AllowSpecificOrigins");
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapHub<NotificationHub>("/notifications");
            app.UseSerilogRequestLogging();
            app.MapControllers();

            app.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application startup failed");
        }
        finally
        {
            Log.CloseAndFlush();
        }
        
    }

    public static async Task DevTalkSeeder(WebApplication app)
    {
        var scope = app.Services.CreateScope();
        var seeder = scope.ServiceProvider.GetRequiredService<IDevTalkSeeder>();
        await seeder.Seed();
    }
}
