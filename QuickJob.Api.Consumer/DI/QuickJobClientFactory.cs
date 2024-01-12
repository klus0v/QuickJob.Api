using QuickJob.Client;
using QuickJob.DataModel.Configuration;
using IConfigurationProvider = Vostok.Configuration.Abstractions.IConfigurationProvider;


namespace QuickJob.Api.Consumer.DI;

public class QuickJobClientFactory
{
    private readonly IConfigurationProvider configuration;

    public QuickJobClientFactory(IConfigurationProvider configuration) => 
        this.configuration = configuration;
    
    public QuickJobClient GetClient()
    {
        var settings = configuration.Get<ServiceSettings>();

        return new QuickJobClient(settings.ApiBaseUrl, settings.ApiKey);
    }
}