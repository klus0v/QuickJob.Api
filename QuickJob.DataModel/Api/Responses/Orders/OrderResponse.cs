using QuickJob.DataModel.Api.Responses.Responses;
using QuickJob.DataModel.Postgres.Entities;

namespace QuickJob.DataModel.Api.Responses.Orders;

public sealed class OrderResponse
{
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
    public bool CurrentUserIsCustomer { get;  set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Address { get; set; }
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
    public List<string>? Categories { get; set; }
    public List<string>? Skills { get; set; }
    public int Limit { get; set; }
    public double WorkHours { get; set; }
    public double Price { get; set; }
    public DateTime CreateDateTime { get; set; }
    public DateTime? EditDateTime { get; set; }
}