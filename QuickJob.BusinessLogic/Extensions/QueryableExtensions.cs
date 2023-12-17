using QuickJob.DataModel.Api.Requests.Orders;
using QuickJob.DataModel.Postgres.Entities;

namespace QuickJob.BusinessLogic.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<Order> OrderByField(this IQueryable<Order> query, SortField? sortField = SortField.CreateDateTime, SortDirection? sortDirection = SortDirection.Ascending)
    {
        return sortField switch
        {
            SortField.EditDateTime => sortDirection == SortDirection.Ascending
                ? query.OrderBy(order => order.EditDateTime)
                : query.OrderByDescending(order => order.EditDateTime),
            SortField.Limit => sortDirection == SortDirection.Ascending
                ? query.OrderBy(order => order.Limit)
                : query.OrderByDescending(order => order.Limit),
            SortField.Price => sortDirection == SortDirection.Ascending
                ? query.OrderBy(order => order.Price)
                : query.OrderByDescending(order => order.Price),
            SortField.ApprovedResponsesCount => sortDirection == SortDirection.Ascending
                ? query.OrderBy(order => order.ApprovedResponsesCount)
                : query.OrderByDescending(order => order.ApprovedResponsesCount),
            SortField.WorkHours => sortDirection == SortDirection.Ascending
                ? query.OrderBy(order => order.WorkHours)
                : query.OrderByDescending(order => order.WorkHours),
            _ => sortDirection == SortDirection.Ascending
                ? query.OrderBy(order => order.CreateDateTime)
                : query.OrderByDescending(order => order.CreateDateTime)
        };
    }
}