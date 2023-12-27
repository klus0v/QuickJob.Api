using QuickJob.DataModel.Api.Responses.Responses;

namespace QuickJob.BusinessLogic.Services;

public interface IResponsesService
{
    Task RespondToOrder(Guid orderId);
    Task DeleteRespondToOrder(Guid responseId);
    Task SetRespondStatus(Guid responseId, ResponseStatus approved);
}