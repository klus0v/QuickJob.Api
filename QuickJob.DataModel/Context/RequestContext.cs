namespace QuickJob.DataModel.Context;

public static class RequestContext
{
    public static void Initialize()
    {
        ClientInfo = new ClientInfo();
    }

    public static ClientInfo ClientInfo
    {
        get => CommonContext.Get<ClientInfo>();
        set => CommonContext.SetValue(value);
    }
}
