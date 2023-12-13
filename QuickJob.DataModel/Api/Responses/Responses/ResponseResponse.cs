namespace QuickJob.DataModel.Api.Responses.Responses;

public class ResponseResponse
{
    public Guid Id { get; set; } 
    public Guid UserId { get; set; } 
    public Guid OrderId { get; set; } 
    public ResponseStatuses Status { get; set; } 
}