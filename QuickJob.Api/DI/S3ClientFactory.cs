using Amazon.S3;
using QuickJob.DataModel.Configuration;
using IConfigurationProvider = Vostok.Configuration.Abstractions.IConfigurationProvider;

namespace QuickJob.Api.DI;

internal class S3ClientFactory
{
    private readonly IConfigurationProvider configuration;

    public S3ClientFactory(IConfigurationProvider configuration) => 
        this.configuration = configuration;

    public AmazonS3Client GetClient()
    {
        var storageSettings = configuration.Get<StorageSettings>();
        return new AmazonS3Client(storageSettings.AccessKeyId, storageSettings.SecretKey, new AmazonS3Config
        {
            ServiceURL = storageSettings.ServiceURL
        });
    }
}
