﻿using DevTalk.Domain.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DevTalk.Domain.Extensions;

public static class ServiceCollectionsExtensions
{
    public static void AddDomain(this IServiceCollection services,IConfiguration configuration)
    {
        services.Configure<BlobStorageOptions>(configuration.GetSection("BlobStorage"));
        services.Configure<GoogleOauthOptions>(configuration.GetSection("GoogleOauth"));
    }
}
