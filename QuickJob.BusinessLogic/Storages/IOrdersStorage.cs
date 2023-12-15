using QuickJob.DataModel.Postgres.Entities;

namespace QuickJob.BusinessLogic.Storages;

public interface IOrdersStorage
{
    IAsyncEnumerable<Order> GetByCustomerId(Guid customerId);
    Task<EntityResult<Order>> GetOrderById(Guid orderId);
    Task<EntityResult> CreateOrder(Order order);
}