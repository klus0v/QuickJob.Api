using QuickJob.DataModel.Api;
using QuickJob.DataModel.Postgres.Entities;

namespace QuickJob.BusinessLogic.Storages;

public interface IResponsesStorage
{
    Task<EntityResult<IReadOnlyList<Response>>> GetResponsesByOrderId(Guid orderId);
    Task<EntityResult<IReadOnlyList<Response>>> GetResponsesByUserId(Guid userId);
}