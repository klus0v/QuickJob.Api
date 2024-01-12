using MassTransit;
using Microsoft.Extensions.DependencyInjection.Extensions;
using QuickJob.Client;
using QuickJob.DataModel.Configuration;
using QuickJob.Users.Client;
using Vostok.Configuration.Sources.Json;
using Vostok.Logging.Abstractions;
using Vostok.Logging.Console;
using Vostok.Logging.File;
using Vostok.Logging.File.Configuration;
using ConfigurationProvider = Vostok.Configuration.ConfigurationProvider;
using IConfigurationProvider = Vostok.Configuration.Abstractions.IConfigurationProvider;

namespace QuickJob.Api.Consumer.DI;

internal static class ServiceCollectionExtensions
{
    public static void AddSettings(this IServiceCollection services)
    {
        var provider = new ConfigurationProvider();
        
        provider.SetupSourceFor<ServiceSettings>(new JsonFileSource($"QuickJob.Settings/{nameof(ServiceSettings)}.json"));
        provider.SetupSourceFor<RabbitMQSettings>(new JsonFileSource($"QuickJob.Settings/{nameof(RabbitMQSettings)}.json"));
        
        services.AddSingleton<IConfigurationProvider>(provider);
    }

    public static void AddSystemServices(this IServiceCollection services) => services
        .AddDistributedMemoryCache();
        
    
    public static void AddExternalServices(this IServiceCollection services)
    {
        services
            .AddSingleton<ILog>(new CompositeLog(new ConsoleLog(), new FileLog(new FileLogSettings())));
        services
            .AddSingleton<UsersClientFactory>()
            .TryAddSingleton<IQuickJobUsersClient>(x => x.GetRequiredService<UsersClientFactory>().GetClient());
        services
            .AddSingleton<QuickJobClientFactory>()
            .TryAddSingleton<IQuickJobClient>(x => x.GetRequiredService<QuickJobClientFactory>().GetClient());
    }

    public static void AddRabbitMq(this IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var rabbitMqSettings = serviceProvider.GetRequiredService<IConfigurationProvider>().Get<RabbitMQSettings>();        

        services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();
            x.SetInMemorySagaRepositoryProvider();

            var assembly = typeof(Program).Assembly;

            x.AddConsumers(assembly);
            x.AddSagaStateMachines(assembly);
            x.AddSagas(assembly);
            x.AddActivities(assembly);
   
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(rabbitMqSettings.HostName,  "/", host =>
                {
                    host.Username(rabbitMqSettings.UserName);
                    host.Password(rabbitMqSettings.Password);
                });
                cfg.ConfigureEndpoints(context);
            });
        });
    }
}
