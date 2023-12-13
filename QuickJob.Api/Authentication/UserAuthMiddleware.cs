using QuickJob.BusinessLogic.Extensions;
using Vostok.Logging.Abstractions;
using QuickJob.DataModel.Context;

namespace QuickJob.Api.Authentication;

internal sealed class UserAuthMiddleware
{
    private readonly RequestDelegate next;
    private readonly ILog log;

    public UserAuthMiddleware(RequestDelegate next, ILog log)
    {
        this.log = log.ForContext<UserAuthMiddleware>();
        this.next = next;
    }
    
    public async Task InvokeAsync(HttpContext context)
    {
        CommonContext.Initialize();
        RequestContext.Initialize();

        var userId = AuthenticateUser(context);
        if (userId == null)
        {
            RequestContext.ClientInfo.IsUserAuthenticated = false;
            await next.Invoke(context);
            return;
        }

        RequestContext.ClientInfo.IsUserAuthenticated = true;
        RequestContext.ClientInfo.UserId = userId.Value;
        await next.Invoke(context);
    }

    private Guid? AuthenticateUser(HttpContext context)
    {
        if (Guid.TryParse(context.User.GetId(), out var id))
        {
            log.Info("Success authenticated");
            return id;
        }
        return null;
    }
}

