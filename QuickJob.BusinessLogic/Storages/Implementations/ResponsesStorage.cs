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

    public async Task<EntityResult<List<Response>>> GetResponsesByOrderId(Guid orderId)
    {
        throw new NotImplementedException();
    }
}