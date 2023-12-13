using QuickJob.DataModel.Api.Requests.Orders;
using QuickJob.DataModel.Entites.Base;

namespace QuickJob.DataModel.Entites;

public class Order : BaseOrder
{
    public Order(CreateOrderRequest createOrderRequest)
    {
        Title = createOrderRequest.Title;
        Limit = createOrderRequest.Limit;
        Price = createOrderRequest.Price;
    }

    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public int ResponsesCount { get; set; }

}