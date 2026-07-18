using DevTalk.API.Middlewares;
using DevTalk.Application.Extensions;
using DevTalk.Domain.Helpers;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

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

        var otelResourceBuilder = ResourceBuilder.CreateDefault()
            .AddService(builder.Environment.ApplicationName);

        builder.Services.AddOpenTelemetry()
            .WithMetrics(metrics =>
            {
                metrics.SetResourceBuilder(otelResourceBuilder)
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddRuntimeInstrumentation()
                .AddMeter("DevTalk")
                .AddOtlpExporter(otlp =>
                {
                    otlp.Endpoint = new Uri("http://otel-collector:4317");
                    otlp.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
                });
            })
            .WithTracing(tracing =>
            {
                tracing
                    .SetResourceBuilder(otelResourceBuilder)
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddOtlpExporter(otlp =>
                    {
                        otlp.Endpoint = new Uri("http://otel-collector:4317");
                        otlp.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
                    });
            });

        

    }
}
