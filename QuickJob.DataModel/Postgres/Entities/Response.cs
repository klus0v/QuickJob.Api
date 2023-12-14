using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using QuickJob.DataModel.Api;
using QuickJob.DataModel.Api.Requests.Orders;

namespace QuickJob.DataModel.Postgres.Entities;

[Table("responses")]
[Index(nameof(OrderId))]
[Index(nameof(UserId))]
public class Response
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public Guid UserId { get; set; }
    public string UserFio { get; set; }
    public ResponseStatuses Status { get; set; }
}