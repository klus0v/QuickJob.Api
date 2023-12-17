using QuickJob.DataModel.Api;
using QuickJob.DataModel.Api.Requests.Orders;
using QuickJob.DataModel.Api.Responses;
using QuickJob.DataModel.Postgres.Entities;

namespace QuickJob.BusinessLogic.Storages;

public interface IOrdersStorage
{
    Task<EntityResult<Order>> GetOrderById(Guid orderId);
    Task<EntityResult> CreateOrder(Order order);
    Task<EntityResult> DeleteOrderById(Order order);
    Task<EntityResult> UpdateOrder(Order order);
    Task<EntityResult<Order>> GetOnlyOrderById(Guid orderId);
    Task<EntityResult<List<Order>>> SearchOrders(SearchOrdersRequest searchOrdersRequest);
}