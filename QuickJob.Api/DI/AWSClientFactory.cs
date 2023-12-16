using Amazon.S3;
using QuickJob.DataModel.Configuration;
using IConfigurationProvider = Vostok.Configuration.Abstractions.IConfigurationProvider;

namespace QuickJob.Api.DI;

internal class AWSClientFactory
{
    private readonly IConfigurationProvider configuration;

    public AWSClientFactory(IConfigurationProvider configuration) => 
        this.configuration = configuration;

    public AmazonS3Client GetClient()
    {
        var storageSettings = configuration.Get<S3Settings>();
        return new AmazonS3Client(storageSettings.AccessKeyId, storageSettings.SecretKey,
            new AmazonS3Config
            {
                ServiceURL = storageSettings.ServiceURL
            });
    }
}