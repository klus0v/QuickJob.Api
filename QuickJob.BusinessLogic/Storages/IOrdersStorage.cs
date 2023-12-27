using QuickJob.DataModel.Api;
using QuickJob.DataModel.Api.Requests.Orders;
using QuickJob.DataModel.Postgres.Entities;

namespace QuickJob.BusinessLogic.Storages;

public interface IOrdersStorage : IBaseEntityStorage<Order>
{
    Task<EntityResult<Order>> GetFullOrderById(Guid orderId);
    Task<EntityResult<List<Order>>> GetOrdersByCustomer(Guid customerId);
    Task<EntityResult<List<Order>>> SearchOrders(SearchOrdersRequest searchOrdersRequest);
}