using QuickJob.DataModel.Api;

namespace QuickJob.BusinessLogic.Storages;

public interface IBaseEntityStorage<TEntity>
{
    Task<EntityResult<TEntity>> GetEntityById(Guid id);
    Task<EntityResult> CreateEntity(TEntity entity);
    Task<EntityResult> DeleteEntity(TEntity entity);
    Task<EntityResult> UpdateEntity(TEntity entity);
}