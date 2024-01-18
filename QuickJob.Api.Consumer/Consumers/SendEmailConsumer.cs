using FS.Keycloak.RestApiClient.Model;
using MassTransit;
using QuickJob.Api.Consumer.Extensions;
using QuickJob.Api.Consumer.Models;
using QuickJob.Client;
using QuickJob.DataModel.Configuration;
using QuickJob.DataModel.RabbitMQ;
using QuickJob.Notifications.Client;
using QuickJob.Notifications.Client.Models.API.Requests.Email;
using QuickJob.Users.Client.Clients;
using Vostok.Logging.Abstractions;

namespace QuickJob.Api.Consumer.Consumers;

public sealed class ApprovedRespondConsumer: IConsumer<ApprovedRespondEvent>
{
    private readonly ILog log;
    private readonly IUsersClient usersClient;
    private readonly ServiceSettings settings;
    private readonly IQuickJobNotificationsClient notificationsClient;
    private readonly IQuickJobClient quickJobClient;

    public ApprovedRespondConsumer(ILog log, IUsersClient usersClient, IQuickJobNotificationsClient notificationsClient, ServiceSettings settings, IQuickJobClient quickJobClient)
    {
        this.usersClient = usersClient;
        this.notificationsClient = notificationsClient;
        this.settings = settings;
        this.quickJobClient = quickJobClient;
        this.log = log.ForContext(nameof(ApprovedRespondConsumer));
    }
    
    public async Task Consume(ConsumeContext<ApprovedRespondEvent> context)
    {
        var user = await usersClient.GetUserAsync(context.Message.UserId);
        //var order = await quickJobClient.Orders.GetOrderAsync(context.Message.OrderId);
        
        if (!user.IsSuccessful)// || !order.IsSuccessful)
        {
            log.Error($"Scip send message: {context.MessageId}");
            return;
        }        
        
        switch (user.Response.GetNotificationChanel())
        {
            case ChanelType.Email:
                var request = GetEmailRequest(user.Response, "order.Response.Title");
                await notificationsClient.Email.SendEmailAsync(request);
                break;
            
            case ChanelType.Telegram:
                break;
            
            case ChanelType.Push:
                break;
            
            case ChanelType.Sms:
                break;
            
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private SendEmailRequest GetEmailRequest(UserRepresentation user, string title) => new()
    {
        Email = user.Email, 
        TemplateName = settings.Templates.ApprovedRespondEmail,
        Variables = new Dictionary<string, string>
        {
            {"fio", user.GetFio()},
            {"order-title", title}
        }
    };
}