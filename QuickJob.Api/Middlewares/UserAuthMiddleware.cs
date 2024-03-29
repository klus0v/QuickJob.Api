using QuickJob.BusinessLogic.Extensions;
using QuickJob.DataModel.Context;
using Vostok.Logging.Abstractions;

namespace QuickJob.Api.Middlewares;

internal sealed class UserAuthMiddleware :  IMiddleware
{
    private readonly ILog log;

    public UserAuthMiddleware(ILog log)
    {
        this.log = log;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
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
        RequestContext.ClientInfo.Name = context.User.GetName();
        await next.Invoke(context);
    }
    
    private Guid? AuthenticateUser(HttpContext context) => 
        Guid.TryParse(context.User.GetId(), out var id) ? id : null;
}

