namespace QuickJob.DataModel.Postgres.Entities.Base;

public class BaseOrder
{
    public string Title { get; set; }
    public int Limit { get; set; }
    public double Price { get; set; }
}