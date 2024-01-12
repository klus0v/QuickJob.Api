using System;

namespace QuickJob.Client.Models.API.Responses;

public class ResponseResponse
{
    public Guid Id { get; set; } 
    public Guid UserId { get; set; }
    public Guid OrderId { get; set; }
    public string UserFio { get; set; }
    public string Status { get; set; } 
}