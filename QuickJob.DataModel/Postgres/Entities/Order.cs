using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using QuickJob.DataModel.Api.Responses.Orders;

namespace QuickJob.DataModel.Postgres.Entities;

[Table("orders")]
[Index(nameof(CustomerId))]
public class Order
{
    public Order()
    {
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; set; }
    
    public string Title { get; set; }
    
    public string Description { get; set; }
    
    public string Address { get; set; }
    
    
    [Column(TypeName = "text[]")]
    public List<string>? Categories { get; set; }

    public List<string>? Skills { get; set; }
    
    public DateTime StartDateTime { get; set; }
    
    public DateTime EndDateTime { get; set; }
    
    public int Limit { get; set; }
    
    public PaymentTypes PaymentType { get; set; }
    
    public double WorkHours { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    public double Price { get; set; }
    
    public Guid CustomerId { get; set; }
    
    public int ResponsesCount { get; set; }
    
    
    [Column(TypeName = "text[]")]
    public List<string> FileUrls { get; set; } 
    
    public DateTime CreateDateTime { get; set; }
    
    public DateTime? EditDateTime { get; set; }
}