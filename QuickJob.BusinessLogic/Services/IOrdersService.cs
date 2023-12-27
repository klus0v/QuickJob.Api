using QuickJob.DataModel.Api.Requests.Orders;
using QuickJob.DataModel.Api.Responses.Orders;

namespace QuickJob.BusinessLogic.Services;

public interface IOrdersService
{
    Task<OrderResponse> CreateOrder(CreateOrderRequest createOrderRequest);
    Task<OrderResponse> GetOrder(Guid orderId);
    Task<OrderResponse> UpdateOrder(Guid orderId, UpdateOrderRequest updateOrderRequest);
    Task DeleteOrder(Guid orderId);
    Task<List<OrderResponse>> SearchOrders(SearchOrdersRequest searchOrdersRequest);
    Task<SearchOrdersResponse> GetOrdersHistory(HistoryType historyType);
}