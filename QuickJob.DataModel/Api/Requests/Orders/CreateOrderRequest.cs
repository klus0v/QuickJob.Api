using Microsoft.AspNetCore.Http;
using QuickJob.DataModel.Api.Base;

namespace QuickJob.DataModel.Api.Requests.Orders;

public sealed class CreateOrderRequest : BaseOrder
{
    public List<IFormFile>? Files { get; set; }
}