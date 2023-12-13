using QuickJob.DataModel.Api;
using QuickJob.DataModel.Api.Requests.Orders;
using QuickJob.DataModel.Api.Responses.Orders;

namespace QuickJob.BusinessLogic.Services;

public interface IOrdersService
{
    Task<OrderResponse> CreateOrder(CreateOrderRequest createOrderRequest);
    Task<OrderResponse> GetOrder(Guid orderId);
    Task<OrderResponse> UpdateOrder(Guid orderId, UpdateOrderRequest updateOrderRequest);
    Task DeleteOrder(Guid orderId);
    Task<SearchOrdersResponse> SearchOrders(SearchOrdersRequest searchOrdersRequest);
    Task RespondToOrder(Guid orderId);
    Task DeleteRespondToOrder(Guid responseId);
    Task SetRespondStatus(Guid responseId, ResponseStatuses approved);
    Task<SearchOrdersResponse> GetOrdersHistory(HistoryTypes historyType);
}