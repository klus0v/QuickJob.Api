namespace QuickJob.DataModel.Api.Requests.Orders;

public sealed class SearchOrdersRequest
{
    public string SortField { get; set; }
    public string SortDirection { get; set; }
    public string SearchQuery { get; set; }
    public List<string> Tags { get; set; }
    public int Take { get; set; } = 50;
    public int Skip { get; set; } = 0;
}