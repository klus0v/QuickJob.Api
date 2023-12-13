using QuickJob.DataModel.Api.Responses.Responses;
using QuickJob.DataModel.Entites.Base;

namespace QuickJob.DataModel.Api.Responses.Orders;

public sealed class OrderResponse : BaseOrder
{
    public Guid Id { get; set; }
    public int ResponsesCount { get; set; }
    public Guid CustomerId { get; set; }
    public List<ResponseResponse> Responses { get; set; }
}