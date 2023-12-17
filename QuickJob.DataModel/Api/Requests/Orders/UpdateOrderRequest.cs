using Microsoft.AspNetCore.Http;
using QuickJob.DataModel.Api.Responses.Orders;

namespace QuickJob.DataModel.Api.Requests.Orders;

public sealed class UpdateOrderRequest
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Address { get; set; }
    public DateTime? StartDateTime { get; set; }
    public DateTime? EndDateTime { get; set; }
    public List<string>? Categories { get; set; }
    public List<string>? Skills { get; set; }
    public int? Limit { get; set; }
    public PaymentType? PaymentType { get; set; }
    public double? WorkHours { get; set; }
    public double? Price { get; set; }
    public bool? IsActive { get; set; }
    public List<string>? FileUrls { get; set; } 
    public List<IFormFile>? NewFiles { get; set; }
}