namespace QuickJob.DataModel.Context;

public sealed class ClientInfo
{
    public bool IsUserAuthenticated { get; set; }
    public Guid UserId { get; set; }
}
