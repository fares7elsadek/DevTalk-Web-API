using DevTalk.API.Middlewares;
using DevTalk.Application.Extensions;
using DevTalk.Domain.Helpers;
using Microsoft.OpenApi.Models;

namespace DevTalk.API.Extensions;

public static class  ServiceCollectionExtensions
{
    public static void AddPresentation(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<ErrorHandlingMiddleware>();
        builder.Host.SeriLogConfigurations();
        builder.Services.AddControllers();
        builder.Services.Configure<EmailOptions>(builder.Configuration.GetSection("email"));

        builder.Services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition("BearerAuth", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference {Type = ReferenceType.SecurityScheme , Id="BearerAuth"}
                    },
                    []
                }
            });
        });

        builder.Services.AddCors( opt =>
        {
            opt.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
            });
        });
    }
}
