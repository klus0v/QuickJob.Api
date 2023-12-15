using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using QuickJob.BusinessLogic.Services;
using QuickJob.BusinessLogic.Services.Implementations;
using QuickJob.BusinessLogic.Storages;
using QuickJob.BusinessLogic.Storages.Implementations;
using QuickJob.DataModel.Configuration;
using QuickJob.DataModel.Context;
using QuickJob.DataModel.Postgres;
using Vostok.Configuration.Sources.Json;
using Vostok.Logging.Abstractions;
using Vostok.Logging.Console;
using Vostok.Logging.File;
using Vostok.Logging.File.Configuration;
using ConfigurationProvider = Vostok.Configuration.ConfigurationProvider;
using IConfigurationProvider = Vostok.Configuration.Abstractions.IConfigurationProvider;

namespace QuickJob.Api.DI;

internal static class ServiceCollectionExtensions
{
    private const string FrontSpecificOrigins = "_frontSpecificOrigins";

    public static void AddServiceCors(this IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var serviceProperties = serviceProvider.GetRequiredService<IConfigurationProvider>().Get<ServiceSettings>();

        services
            .AddCors(option => option
                .AddPolicy(FrontSpecificOrigins, builder => builder.WithOrigins(serviceProperties.Origins.ToArray())
                    .AllowAnyMethod()
                    .AllowAnyHeader()));
    }
    
    public static void AddServiceAuthentication(this IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var keycloackSettings = serviceProvider.GetRequiredService<IConfigurationProvider>().Get<KeycloackSettings>();
        
        services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.Authority = keycloackSettings.Authority;
                options.Audience = keycloackSettings.Audience;

                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = c =>
                    {
                        var userIdClaim = c.Principal.Claims.FirstOrDefault(x => x.Type == keycloackSettings.SubClaim);
                        if (userIdClaim != null)
                        {
                            RequestContext.ClientInfo.IsUserAuthenticated = true;
                            RequestContext.ClientInfo.UserId = Guid.Parse(userIdClaim.Value);
                        }
                        return Task.CompletedTask;
                    }
                };
            });
    }
    
    public static void AddPostgresStorage(this IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var postgresSettings = serviceProvider.GetRequiredService<IConfigurationProvider>().Get<PostgresSettings>();

        services.AddDbContext<QuickJobContext>(options =>
        {
            options.UseNpgsql(postgresSettings.DbConnectionString);
        }, optionsLifetime: ServiceLifetime.Singleton);
    }
    
    public static void AddServiceSwaggerDocument(this IServiceCollection services)
    {
        services.AddSwaggerDocument(doc =>
        {
            doc.Title = "QuickJob.Api";
            doc.AddSecurity("Bearer", Enumerable.Empty<string>(), new NSwag.OpenApiSecurityScheme
            {
                Type = NSwag.OpenApiSecuritySchemeType.ApiKey,
                Name = "Authorization",
                In = NSwag.OpenApiSecurityApiKeyLocation.Header,
                Description = "Type into the textbox: Bearer {your JWT token}."
            });
        });
    }

    public static void AddSettings(this IServiceCollection services)
    {
        var provider = new ConfigurationProvider();

        provider.SetupSourceFor<ServiceSettings>(new JsonFileSource("Properties/ServiceSettings.json"));
        provider.SetupSourceFor<StorageSettings>(new JsonFileSource("Properties/StorageSettings.json"));
        provider.SetupSourceFor<PostgresSettings>(new JsonFileSource("Properties/PostgresSettings.json"));
        provider.SetupSourceFor<SmtpSettings>(new JsonFileSource("Properties/SmtpSettings.json"));
        provider.SetupSourceFor<KeycloackSettings>(new JsonFileSource("Properties/KeycloackSettings.json"));

        services.AddSingleton<IConfigurationProvider>(provider);
    }

    public static void AddSystemServices(this IServiceCollection services) => services
        .AddDistributedMemoryCache()
        .AddSingleton<IOrdersService, OrdersService>()
        .AddSingleton<IOrdersStorage, OrdersStorage>()
        .AddSingleton<IResponsesStorage, ResponsesStorage>();

    public static void AddExternalServices(this IServiceCollection services)
    {
        services
            .AddSingleton<ILog>(new CompositeLog(new ConsoleLog(), new FileLog(new FileLogSettings())));
        services
            .AddSingleton<S3ClientFactory>()
            .TryAddSingleton(x => x.GetRequiredService<S3ClientFactory>().GetClient());
        services
            .AddSingleton<SmtpClientFactory>()
            .TryAddSingleton(x => x.GetRequiredService<SmtpClientFactory>().GetClient());
    }
}
