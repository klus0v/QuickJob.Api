using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using QuickJob.DataModel.Configuration;
using IConfigurationProvider = Vostok.Configuration.Abstractions.IConfigurationProvider;

namespace QuickJob.Api.DI;

internal class MongoFactory
{
    private readonly IConfigurationProvider configuration;

    public MongoFactory(IConfigurationProvider configuration) => 
        this.configuration = configuration;

    public IMongoClient GetClient()
    {
        var mongoSettings = configuration.Get<MongoSettings>();
        var settings = new MongoClientSettings()
        {
            Scheme = ConnectionStringScheme.MongoDB,
            Server = new MongoServerAddress(mongoSettings.Host, mongoSettings.Port),
            ConnectTimeout =  TimeSpan.Parse(mongoSettings.Timeout),
            UseTls = mongoSettings.UseTls
        };
        return new MongoClient(settings);
    }
}
