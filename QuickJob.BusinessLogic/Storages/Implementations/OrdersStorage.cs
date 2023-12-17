using Microsoft.EntityFrameworkCore;
using QuickJob.DataModel.Api;
using QuickJob.DataModel.Postgres;
using QuickJob.DataModel.Postgres.Entities;
using Vostok.Logging.Abstractions;

namespace QuickJob.BusinessLogic.Storages.Implementations;

public sealed class OrdersStorage : IOrdersStorage
{
    private readonly Func<QuickJobContext> dbContextFactory;
    private readonly ILog log;

    public OrdersStorage(DbContextOptions<QuickJobContext> dbContextOptions, ILog log)
    {
        this.log = log.ForContext(nameof(OrdersStorage));
        dbContextFactory = () => new QuickJobContext(dbContextOptions);
    }

    public async IAsyncEnumerable<Order> GetByCustomerId(Guid customerId)
    {
        await using var dbContext = dbContextFactory();
        var query = dbContext
            .Set<Order>()
            .Where(s => s.CustomerId == customerId)
            .AsAsyncEnumerable();

        await foreach (var statistic in query)
            yield return statistic;
    }

    public async Task<EntityResult<Order>> GetOrderById(Guid orderId)
    {
        try
        {
            await using var dbContext = dbContextFactory();
            var order = await dbContext
                .Set<Order>()
                .FirstOrDefaultAsync(x => x.Id == orderId);

            return EntityResult<Order>.CreateSuccessful(order);
        }
        catch (Exception e)
        {
            log.Error($"Get order: '{orderId}' fail with error: '{e.Message}'; StackTrace: '{e.StackTrace}'.");
            return EntityResult<Order>.CreateError(new ErrorResult(e.Message, e.HResult));
        }
       
    }

    public async Task<EntityResult> CreateOrder(Order order)
    {
        try
        {
            await using var dbContext = dbContextFactory();
            await dbContext
                .Set<Order>()
                .AddAsync(order);
            await dbContext.SaveChangesAsync();

            return EntityResult.CreateSuccessful();
        }
        catch (Exception e)
        {
            log.Error($"Create order: '{order.Id}' fail with error: '{e.Message}'; StackTrace: '{e.StackTrace}'.");
            return EntityResult<Order>.CreateError(new ErrorResult(e.Message, e.HResult));
        }
    }

    public async Task<EntityResult> DeleteOrderById(Order order)
    {
        try
        {
            await using var dbContext = dbContextFactory();
            dbContext
                .Set<Order>()
                .Remove(order);
            await dbContext.SaveChangesAsync();

            return EntityResult.CreateSuccessful();
        }
        catch (Exception e)
        {
            log.Error($"Delete order: '{order.Id}' fail with error: '{e.Message}'; StackTrace: '{e.StackTrace}'.");
            return EntityResult.CreateError(new ErrorResult(e.Message, e.HResult));
        }
    }

    public async Task<EntityResult> UpdateOrder(Order order)
    {
        try
        {
            await using var dbContext = dbContextFactory();
            dbContext
                .Set<Order>()
                .Update(order);
            await dbContext.SaveChangesAsync();

            return EntityResult.CreateSuccessful();
        }
        catch (Exception e)
        {
            log.Error($"Update order: '{order.Id}' fail with error: '{e.Message}'; StackTrace: '{e.StackTrace}'.");
            return EntityResult<Order>.CreateError(new ErrorResult(e.Message, e.HResult));
        }
    }
}