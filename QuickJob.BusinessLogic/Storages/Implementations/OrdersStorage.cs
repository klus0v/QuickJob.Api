using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using QuickJob.BusinessLogic.Extensions;
using QuickJob.DataModel.Api;
using QuickJob.DataModel.Api.Requests.Orders;
using QuickJob.DataModel.Api.Responses;
using QuickJob.DataModel.Context;
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

    public async Task<EntityResult<Order>> GetOnlyOrderById(Guid orderId)
    {
        try
        {
            await using var dbContext = dbContextFactory();
            var order = await dbContext
                .Set<Order>()
                .FirstOrDefaultAsync(o => o.Id == orderId);

            return EntityResult<Order>.CreateSuccessful(order);
        }
        catch (Exception e)
        {
            log.Error($"Get only order: '{orderId}' fail with error: '{e.Message}'; StackTrace: '{e.StackTrace}'.");
            return EntityResult<Order>.CreateError(new ErrorResult(e.Message, e.HResult));
        }
    }

    public async Task<EntityResult<List<Order>>> SearchOrders(SearchOrdersRequest searchOrdersRequest)
    {
        try
        {
            await using var dbContext = dbContextFactory();
            var orders = await dbContext
                .Set<Order>()
                .Include(o => o.Responses)
                .OrderByField(searchOrdersRequest.SortField, searchOrdersRequest.SortDirection)
                .Where(ApplySearchFilters(searchOrdersRequest))
                .Skip(searchOrdersRequest.Skip)
                .Take(searchOrdersRequest.Take)
                .ToListAsync();

            return EntityResult<List<Order>>.CreateSuccessful(orders);
        }
        catch (Exception e)
        {
            log.Error($"Get orders fail with error: '{e.Message}'; StackTrace: '{e.StackTrace}'.");
            return EntityResult<List<Order>>.CreateError(new ErrorResult(e.Message, e.HResult));
        }
    }

    private static Expression<Func<Order, bool>> ApplySearchFilters(SearchOrdersRequest request)
    {
        var userId = RequestContext.ClientInfo.UserId;
        List<string> tags;

        return order => order.IsActive
                        && order.Limit > order.ApprovedResponsesCount
                        && order.CustomerId != userId
                        && (order.Responses.Count == 0 || order.Responses.All(r => r.UserId != userId))
                        && (request.Query == null || order.Title.ToLower().Contains(request.Query.ToLower()) || order.Description.ToLower().Contains(request.Query.ToLower()))
                        && (request.TagsList.TryGetList(out tags, default) || order.Categories != null && tags.All(tag => order.Categories.Contains(tag)))
                        && (request.PaymentType == null || order.PaymentType == request.PaymentType)
                        && (!request.StartDateTimeAfter.HasValue || order.StartDateTime > request.StartDateTimeAfter.Value)
                        && (!request.EndDateTimeBefore.HasValue || order.EndDateTime < request.EndDateTimeBefore.Value);
    }


    public async Task<EntityResult<Order>> GetOrderById(Guid orderId)
    {
        try
        {
            await using var dbContext = dbContextFactory();
            var order = await dbContext
                .Set<Order>()
                .Include(o => o.Responses)
                .FirstOrDefaultAsync(o => o.Id == orderId);

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