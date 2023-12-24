using QuickJob.DataModel.Api.Requests.Orders;
using QuickJob.DataModel.Api.Responses.Orders;
using QuickJob.DataModel.Api.Responses.Responses;
using QuickJob.DataModel.Postgres.Entities;

namespace QuickJob.BusinessLogic.Mappers;

public static class ResponseMapper
{
    public static ResponseResponse ToResponse(this Response response)
    {
        return new ResponseResponse
        {
            
        };
    }
    public static Response CreateRespondToEntity(this Guid orderId, Guid userId, string userFio)
    {
        return new Response
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            UserFio = userFio,
            OrderId = orderId,
            Status = ResponseStatus.Requested
        };
    }
}