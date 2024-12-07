using DevTalk.API.Extensions;
using DevTalk.API.Middlewares;
using DevTalk.Application.Extensions;
using DevTalk.Domain.Entites;
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
        builder.Services.AddPresentation();
        builder.Host.SeriLogConfigurations();
        builder.Services.AddApplication();
        builder.Services.AddControllers()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });
        
        builder.Services.AddOpenApi();

        var app = builder.Build();
        DevTalkSeeder(app);

        app.UseMiddleware<ErrorHandlingMiddleware>();
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "v1"));
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(
           Path.Combine(builder.Environment.ContentRootPath, "Uploads")),
            RequestPath = "/Resources"
        });
        app.UseCors();
        app.MapGroup("api/identity")
            .WithTags("Identity").MapIdentityApi<User>();
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
