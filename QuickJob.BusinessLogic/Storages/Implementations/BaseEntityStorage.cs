using Microsoft.EntityFrameworkCore;
using QuickJob.DataModel.Api;
using QuickJob.DataModel.Postgres;
using QuickJob.DataModel.Postgres.Entities;
using Vostok.Logging.Abstractions;

namespace QuickJob.BusinessLogic.Storages.Implementations;

public abstract class BaseEntityStorage<TEntity> : IBaseEntityStorage<TEntity> where TEntity : BaseEntity
{
    protected readonly Func<QuickJobContext> dbContextFactory;
    protected readonly ILog log;

    protected BaseEntityStorage(DbContextOptions<QuickJobContext> dbContextOptions, ILog log)
    {
        this.log = log.ForContext(nameof(TEntity));
        dbContextFactory = () => new QuickJobContext(dbContextOptions);
    }

    public async Task<EntityResult<TEntity>> GetEntityById(Guid id)
    {
        try
        {
            await using var dbContext = dbContextFactory();
            var entity = await dbContext
                .Set<TEntity>()
                .FirstOrDefaultAsync(o => o.Id == id);

            return EntityResult<TEntity>.CreateSuccessful(entity);
        }
        catch (Exception e)
        {
            log.Error($"Get {nameof(TEntity)}: '{id}' fail with error: '{e.Message}'; StackTrace: '{e.StackTrace}'.");
            return EntityResult<TEntity>.CreateError(new ErrorResult(e.Message, e.HResult));
        }
    }

    public async Task<EntityResult> CreateEntity(TEntity entity)
    {
        try
        {
            await using var dbContext = dbContextFactory();
            await dbContext
                .Set<TEntity>()
                .AddAsync(entity);
            await dbContext.SaveChangesAsync();

            return EntityResult.CreateSuccessful();
        }
        catch (Exception e)
        {
            log.Error($"Create {nameof(TEntity)}: '{entity.Id}' fail with error: '{e.Message}'; StackTrace: '{e.StackTrace}'.");
            return EntityResult<TEntity>.CreateError(new ErrorResult(e.Message, e.HResult));
        }
    }

    public async Task<EntityResult> DeleteEntity(TEntity entity)
    {
        try
        {
            await using var dbContext = dbContextFactory();
            dbContext
                .Set<TEntity>()
                .Remove(entity);
            await dbContext.SaveChangesAsync();

            return EntityResult.CreateSuccessful();
        }
        catch (Exception e)
        {
            log.Error($"Delete {nameof(TEntity)}: '{entity.Id}' fail with error: '{e.Message}'; StackTrace: '{e.StackTrace}'.");
            return EntityResult.CreateError(new ErrorResult(e.Message, e.HResult));
        }
    }

    public async Task<EntityResult> UpdateEntity(TEntity entity)
    {
        try
        {
            await using var dbContext = dbContextFactory();
            dbContext
                .Set<TEntity>()
                .Update(entity);
            await dbContext.SaveChangesAsync();

            return EntityResult.CreateSuccessful();
        }
        catch (Exception e)
        {
            log.Error($"Update {nameof(TEntity)}: '{entity.Id}' fail with error: '{e.Message}'; StackTrace: '{e.StackTrace}'.");
            return EntityResult<TEntity>.CreateError(new ErrorResult(e.Message, e.HResult));
        }
    }
}