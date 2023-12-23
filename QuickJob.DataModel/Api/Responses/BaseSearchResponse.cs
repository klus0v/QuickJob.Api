namespace QuickJob.DataModel.Api.Responses;

public class BaseSearchResponse<T>
{
    public List<T> FoundItems { get; set; }
    public int TotalCount { get; set; }
}