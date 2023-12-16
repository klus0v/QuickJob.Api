using QuickJob.DataModel.Api.Base;
using QuickJob.DataModel.Postgres.Entities;

namespace QuickJob.BusinessLogic.Storages;

public interface IResponsesStorage
{
    Task<EntityResult<IReadOnlyList<Response>>> GetResponsesByOrderId(Guid orderId);
}