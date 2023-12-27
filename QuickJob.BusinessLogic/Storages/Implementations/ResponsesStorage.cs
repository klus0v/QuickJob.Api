using Microsoft.EntityFrameworkCore;
using QuickJob.DataModel.Api;
using QuickJob.DataModel.Postgres;
using QuickJob.DataModel.Postgres.Entities;
using Vostok.Logging.Abstractions;

namespace QuickJob.BusinessLogic.Storages.Implementations;

public sealed class ResponsesStorage : BaseEntityStorage<Response>, IResponsesStorage
{
    public ResponsesStorage(DbContextOptions<QuickJobContext> dbContextOptions, ILog log): base(dbContextOptions, log)
    {
    }

    public async Task<EntityResult<IReadOnlyList<Response>>> GetResponsesByOrderId(Guid orderId)
    {
        try
        {
            await using var dbContext = dbContextFactory();
            var responses = await dbContext
                .Set<Response>()
                .Where(s => s.OrderId == orderId)
                .ToListAsync();

            return EntityResult<IReadOnlyList<Response>>.CreateSuccessful(responses);
        }
        catch (Exception e)
        {
            log.Error($"Get responses for order: '{orderId}' fail with error: '{e.Message}'; StackTrace: '{e.StackTrace}'.");
            return EntityResult<IReadOnlyList<Response>>.CreateError(new ErrorResult(e.Message, e.HResult));
        }
    }

    public async Task<EntityResult<IReadOnlyList<Response>>> GetResponsesByUserId(Guid userId)
    {
        try
        {
            await using var dbContext = dbContextFactory();
            var responses = await dbContext
                .Set<Response>()
                .Include(r => r.Order)
                .Where(s => s.UserId == userId)
                .ToListAsync();

            return EntityResult<IReadOnlyList<Response>>.CreateSuccessful(responses);
        }
        catch (Exception e)
        {
            log.Error($"Get responses for user: '{userId}' fail with error: '{e.Message}'; StackTrace: '{e.StackTrace}'.");
            return EntityResult<IReadOnlyList<Response>>.CreateError(new ErrorResult(e.Message, e.HResult));
        }
    }

}