using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using QuickJob.DataModel.Api.Requests.Orders;

namespace QuickJob.DataModel.Postgres.Entities;

[Table("orders")]
[Index(nameof(CustomerId))]
public class Order
{
    public Order(CreateOrderRequest createOrderRequest, Guid userId)
    {
        Id = Guid.NewGuid();
        CustomerId = userId;
        Title = createOrderRequest.Title;
        Limit = createOrderRequest.Limit;
        Price = createOrderRequest.Price;
    }
    
    public Order(CreateOrderRequest createOrderRequest)
    {
        Title = createOrderRequest.Title;
        Limit = createOrderRequest.Limit;
        Price = createOrderRequest.Price;
    }
    
    public Order()
    {
        
    }

    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; set; }
    public string Title { get; set; }
    public int Limit { get; set; }
    public bool IsActive { get; set; } = true;
    public double Price { get; set; }
    public Guid CustomerId { get; set; }
    public int ResponsesCount { get; set; }
    [Column(TypeName = "text[]")]
    public List<string> FileUrls { get; set; } 
}