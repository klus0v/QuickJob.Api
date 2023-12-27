using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using QuickJob.DataModel.Api.Requests.Orders;
using QuickJob.DataModel.Api.Responses.Orders;

namespace QuickJob.DataModel.Postgres.Entities;

[Table("orders")]
[Index(nameof(CustomerId))]
public class Order : BaseEntity
{
    public Order()
    {
    }

    public string Title { get; set; }
    
    public string Description { get; set; }
    
    public string Address { get; set; }

    public List<string>? Categories { get; set; }

    public List<string>? Skills { get; set; }
    
    public DateTime StartDateTime { get; set; }
    
    public DateTime EndDateTime { get; set; }
    
    public int Limit { get; set; }
    
    public PaymentType PaymentType { get; set; }
    
    public double WorkHours { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    public double Price { get; set; }
    
    public Guid CustomerId { get; set; }
    
    public int ApprovedResponsesCount { get; set; }
    
    public List<string>? FileUrls { get; set; }

    public List<Response> Responses { get; set; } = new();
}