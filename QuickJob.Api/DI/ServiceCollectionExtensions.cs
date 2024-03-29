﻿using System.Text.Json.Serialization;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using QuickJob.Api.Middlewares;
using QuickJob.BusinessLogic.Services;
using QuickJob.BusinessLogic.Services.Implementations;
using QuickJob.BusinessLogic.Storages;
using QuickJob.BusinessLogic.Storages.Implementations;
using QuickJob.BusinessLogic.Storages.S3;
using QuickJob.DataModel.Configuration;
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
                //.AddPolicy(FrontSpecificOrigins, builder => builder.WithOrigins(serviceSettings.Origins.ToArray())
                .AddPolicy(FrontSpecificOrigins, builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()));
    }
    
    public static void UseServiceCors(this IApplicationBuilder builder) => 
        builder.UseCors(FrontSpecificOrigins);

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
            });
    }
    
    public static void AddRabbitMq(this IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var rabbitMqSettings = serviceProvider.GetRequiredService<IConfigurationProvider>().Get<RabbitMQSettings>();        

        services.AddMassTransit(x => x.UsingRabbitMq((context, cfg) =>
        {
            cfg.Host(rabbitMqSettings.HostName,  "/", host =>
            {
                host.Username(rabbitMqSettings.UserName);
                host.Password(rabbitMqSettings.Password);
            });
            cfg.ConfigureEndpoints(context);
        }));
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
        services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });
    }

    public static void AddSettings(this IServiceCollection services)
    {
        var provider = new ConfigurationProvider();

        provider.SetupSourceFor<ServiceSettings>(new JsonFileSource($"QuickJob.Settings/{nameof(ServiceSettings)}.json"));
        provider.SetupSourceFor<S3Settings>(new JsonFileSource($"QuickJob.Settings/{nameof(S3Settings)}.json"));
        provider.SetupSourceFor<PostgresSettings>(new JsonFileSource($"QuickJob.Settings/{nameof(PostgresSettings)}.json"));
        provider.SetupSourceFor<RabbitMQSettings>(new JsonFileSource($"QuickJob.Settings/{nameof(RabbitMQSettings)}.json"));
        provider.SetupSourceFor<KeycloackSettings>(new JsonFileSource($"QuickJob.Settings/{nameof(KeycloackSettings)}.json"));

        services.AddSingleton<IConfigurationProvider>(provider);
    }

    public static void AddSystemServices(this IServiceCollection services) => services
        .AddDistributedMemoryCache()
        .AddSingleton<IOrdersService, OrdersService>()
        .AddSingleton<IResponsesService, ResponsesService>()
        .AddSingleton<IOrdersStorage, OrdersStorage>()
        .AddSingleton<IResponsesStorage, ResponsesStorage>()
        .AddSingleton<IS3Storage, AWSStorage>();

    public static void AddExternalServices(this IServiceCollection services)
    {
        services
            .AddSingleton<ILog>(new CompositeLog(new ConsoleLog(), new FileLog(new FileLogSettings())));
        services
            .AddSingleton<AWSClientFactory>()
            .TryAddSingleton(x => x.GetRequiredService<AWSClientFactory>().GetClient());
    }
    
    public static void AddAuthMiddleware(this IServiceCollection services) =>
        services.AddSingleton<UserAuthMiddleware>();

    public static void UseAuthMiddleware(this IApplicationBuilder builder) =>
        builder.UseMiddleware<UserAuthMiddleware>();

    public static void UseUnhandledExceptionMiddleware(this IApplicationBuilder builder) => 
        builder.UseMiddleware<UnhandledExceptionMiddleware>();
}
