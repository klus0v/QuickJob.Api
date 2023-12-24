using Microsoft.EntityFrameworkCore;
using QuickJob.DataModel.Api;
using QuickJob.DataModel.Postgres;
using QuickJob.DataModel.Postgres.Entities;
using Vostok.Logging.Abstractions;

namespace QuickJob.BusinessLogic.Storages.Implementations;

public sealed class ResponsesStorage : IResponsesStorage
{
    private readonly Func<QuickJobContext> dbContextFactory;
    private readonly ILog log;

    public ResponsesStorage(DbContextOptions<QuickJobContext> dbContextOptions, ILog log)
    {
        this.log = log.ForContext(nameof(ResponsesStorage));
        dbContextFactory = () => new QuickJobContext(dbContextOptions);
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

    public async Task<EntityResult<Response>> GetResponseById(Guid id)
    {
        try
        {
            await using var dbContext = dbContextFactory();
            var response = await dbContext
                .Set<Response>()
                .Include(r => r.Order)
                .FirstOrDefaultAsync(o => o.Id == id);

            return EntityResult<Response>.CreateSuccessful(response);
        }
        catch (Exception e)
        {
            log.Error($"Get response: '{id}' fail with error: '{e.Message}'; StackTrace: '{e.StackTrace}'.");
            return EntityResult<Response>.CreateError(new ErrorResult(e.Message, e.HResult));
        }
    }

    public async Task<EntityResult> DeleteResponse(Response response)
    {
        try
        {
            await using var dbContext = dbContextFactory();
            dbContext
                .Set<Response>()
                .Remove(response);
            await dbContext.SaveChangesAsync();

            return EntityResult.CreateSuccessful();
        }
        catch (Exception e)
        {
            log.Error($"Delete response: '{response.Id}' fail with error: '{e.Message}'; StackTrace: '{e.StackTrace}'.");
            return EntityResult.CreateError(new ErrorResult(e.Message, e.HResult));
        }
    }

    public async Task<EntityResult> CreateResponse(Response response)
    {
        try
        {
            await using var dbContext = dbContextFactory();
            await dbContext
                .Set<Response>()
                .AddAsync(response);
            await dbContext.SaveChangesAsync();

            return EntityResult.CreateSuccessful();
        }
        catch (Exception e)
        {
            log.Error($"Create response: '{response.Id}' fail with error: '{e.Message}'; StackTrace: '{e.StackTrace}'.");
            return EntityResult<Order>.CreateError(new ErrorResult(e.Message, e.HResult));
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

    public async Task<EntityResult> UpdateResponse(Response response)
    {
        try
        {
            await using var dbContext = dbContextFactory();
            dbContext
                .Set<Response>()
                .Update(response);
            await dbContext.SaveChangesAsync();

            return EntityResult.CreateSuccessful();
        }
        catch (Exception e)
        {
            log.Error($"Update response: '{response.Id}' fail with error: '{e.Message}'; StackTrace: '{e.StackTrace}'.");
            return EntityResult<Order>.CreateError(new ErrorResult(e.Message, e.HResult));
        }
    }
}