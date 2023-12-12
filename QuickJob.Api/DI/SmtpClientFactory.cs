using System.Net;
using System.Net.Mail;
using QuickJob.DataModel.Configuration;
using IConfigurationProvider = Vostok.Configuration.Abstractions.IConfigurationProvider;

namespace QuickJob.Api.DI;

internal class SmtpClientFactory
{
    private readonly IConfigurationProvider configuration;

    public SmtpClientFactory(IConfigurationProvider configuration) => 
        this.configuration = configuration;

    public SmtpClient GetClient()
    {
        var smtpSettings = configuration.Get<SmtpSettings>();
        
        return new SmtpClient
        {
            Port = smtpSettings.Port,
            Host = smtpSettings.Host,
            EnableSsl = smtpSettings.EnableSsl,
            UseDefaultCredentials = smtpSettings.UseDefaultCredentials,
            Credentials = new NetworkCredential(smtpSettings.Login, smtpSettings.Password),
            DeliveryMethod = SmtpDeliveryMethod.Network
        };
    }
}
