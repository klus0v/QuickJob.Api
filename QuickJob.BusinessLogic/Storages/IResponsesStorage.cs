using QuickJob.DataModel.Postgres.Entities;

namespace QuickJob.BusinessLogic.Storages;

public interface IResponsesStorage
{
    Task<EntityResult<List<Response>>> GetResponsesByOrderId(Guid orderId);
}