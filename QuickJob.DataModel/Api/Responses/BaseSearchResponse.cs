namespace QuickJob.DataModel.Api.Responses;

public abstract class BaseSearchResponse<T>
{
    public IList<T> FoundItems { get; set; }
    public int TotalCount { get; set; }
}