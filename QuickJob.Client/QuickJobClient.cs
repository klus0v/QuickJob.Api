using System;
using System.Net.Http;
using System.Net.Http.Headers;
using QuickJob.Client.Clients;

namespace QuickJob.Client;

public class QuickJobClient : IQuickJobClient
{
    public QuickJobClient(string baseUrl, string apiKey)
    {
        var httpClient = new HttpClient { BaseAddress = new Uri(baseUrl)};
        httpClient.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("api.key", apiKey);
        var requestSender = new StandaloneRequestSender(httpClient);
        
        Orders = new OrdersClient(requestSender);
    }

    public IOrdersClient Orders { get; }
}
