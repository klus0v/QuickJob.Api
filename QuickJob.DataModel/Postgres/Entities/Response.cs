using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using QuickJob.DataModel.Api.Responses.Responses;

namespace QuickJob.DataModel.Postgres.Entities;

[Table("responses")]
[Index(nameof(OrderId))]
[Index(nameof(UserId))]
public class Response : BaseEntity
{
    public Guid OrderId { get; set; }
    public Guid UserId { get; set; }
    public string UserFio { get; set; }
    public ResponseStatus Status { get; set; }
    
    // Navigation property for the associated order
    [ForeignKey("OrderId")]
    public Order Order { get; set; }
}