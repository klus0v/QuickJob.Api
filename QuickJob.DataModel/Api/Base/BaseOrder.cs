using QuickJob.DataModel.Api.Responses.Orders;

namespace QuickJob.DataModel.Api.Base;

public class BaseOrder
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Address { get; set; }
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
    public List<string>? Categories { get; set; }
    public List<string>? Skills { get; set; }
    public int Limit { get; set; }
    public PaymentTypes PaymentType { get; set; }
    public double WorkHours { get; set; }
    public double Price { get; set; }
}