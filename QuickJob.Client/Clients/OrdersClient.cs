using System;
using System.Threading.Tasks;
using QuickJob.Client.Models;
using QuickJob.Client.Models.API.Responses;

namespace QuickJob.Client.Clients;

public class OrdersClient : IOrdersClient
{
    private readonly IRequestSender sender;

    public OrdersClient(IRequestSender sender)
    {
        this.sender = sender;
    }

    public async Task<ApiResult<OrderResponse>> GetOrderAsync(Guid id) => 
        await sender.SendRequestAsync<OrderResponse>("GET", ClientPaths.Order(id));

}