using QuickJob.DataModel.Api.Responses.Orders;

namespace QuickJob.DataModel.Api.Requests.Orders;

public sealed class SearchOrdersRequest
{
    public string? Query { get; set; }
    public string? TagsList { get; set; } //delimiter - ',' 
    public PaymentType? PaymentType { get; set; }
    public DateTime? StartDateTimeAfter { get; set; }
    public DateTime? EndDateTimeBefore { get; set; }
    public SortField? SortField { get; set; }
    public SortDirection? SortDirection { get; set; }
    public int Take { get; set; } = 50;
    public int Skip { get; set; } = 0;
}

public enum SortDirection
{
    Ascending  = 1,
    Descending  = -1
}

public enum SortField 
{
    CreateDateTime,
    EditDateTime,
    WorkHours,
    Limit,
    ApprovedResponsesCount,
    Price
}
