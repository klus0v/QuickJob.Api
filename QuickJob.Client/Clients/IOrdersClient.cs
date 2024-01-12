using System;
using System.Threading.Tasks;
using QuickJob.Client.Models;
using QuickJob.Client.Models.API.Responses;

namespace QuickJob.Client.Clients;

public interface IOrdersClient
{
    Task<ApiResult<OrderResponse>> GetOrderAsync(Guid id);
}
