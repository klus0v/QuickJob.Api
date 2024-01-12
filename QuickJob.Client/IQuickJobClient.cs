using QuickJob.Client.Clients;

namespace QuickJob.Client;

public interface IQuickJobClient
{
    IOrdersClient Orders { get; }
}
