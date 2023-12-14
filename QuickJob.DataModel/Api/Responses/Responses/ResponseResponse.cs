using QuickJob.DataModel.Postgres.Entities;

namespace QuickJob.DataModel.Api.Responses.Responses;

public class ResponseResponse
{
    public ResponseResponse(Response resp)
    {
        Id = resp.Id;
        OrderId = resp.OrderId;
        UserId = resp.UserId;
        UserFio = resp.UserFio;
        Status = resp.Status;
    }

    public ResponseResponse()
    {
        
    }

    public Guid Id { get; set; } 
    public Guid UserId { get; set; }
    public Guid OrderId { get; set; }
    public string UserFio { get; set; }
    public ResponseStatuses Status { get; set; } 
}