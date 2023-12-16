using QuickJob.DataModel.Api.Base;
using QuickJob.DataModel.Api.Responses.Responses;
using QuickJob.DataModel.Postgres.Entities;

namespace QuickJob.DataModel.Api.Responses.Orders;

public sealed class OrderResponse : BaseOrder
{
    public OrderResponse(Order order)
    {
        Id = order.Id;
        CustomerId = order.CustomerId;
        ResponsesCount = order.ResponsesCount;
        Title = order.Title;
        Description = order.Description;
        Address = order.Address;
        StartDateTime = order.StartDateTime;
        EndDateTime = order.EndDateTime;
        Categories = order.Categories ?? null;
        Skills = order.Skills ?? null;
        Limit = order.Limit;
        PaymentType = order.PaymentType.ToString();
        WorkHours = order.WorkHours;
        Price = order.Price;
        FileUrls = order.FileUrls;
        IsActive = order.IsActive;
    }

    public OrderResponse()
    {
        
    }

    public Guid Id { get; set; }
    public int ResponsesCount { get; set; }
    public Guid CustomerId { get; set; }
    public IEnumerable<ResponseResponse>? Responses { get; set; }
    public List<string> FileUrls { get;  set; } 
    public bool IsActive { get;  set; } 
    public string PaymentType { get;  set; } 

}