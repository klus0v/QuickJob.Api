using Microsoft.EntityFrameworkCore;
using QuickJob.DataModel.Postgres;
using QuickJob.DataModel.Postgres.Entities;
using Vostok.Logging.Abstractions;

namespace QuickJob.BusinessLogic.Storages.Implementations;

public class ResponsesStorage : IResponsesStorage
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
}