using QuickJob.DataModel.Api;
using QuickJob.DataModel.Postgres.Entities;

namespace QuickJob.BusinessLogic.Storages;

public interface IResponsesStorage
{
    Task<EntityResult<IReadOnlyList<Response>>> GetResponsesByOrderId(Guid orderId);
    Task<EntityResult<Response>> GetResponseById(Guid id);
    Task<EntityResult> DeleteResponse(Response response);
    Task<EntityResult> CreateResponse(Response response);
    Task<EntityResult<IReadOnlyList<Response>>> GetResponsesByUserId(Guid userId);
    Task<EntityResult> UpdateResponse(Response response);
}