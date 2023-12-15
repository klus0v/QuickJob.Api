using System.Text.Json;
using NJsonSchema.Annotations;
using QuickJob.DataModel.Exceptions;
using Vostok.Logging.Abstractions;

namespace QuickJob.Api.Middlewares;

public class UnhandledExceptionMiddleware
{
    private readonly ILog log;
    private readonly RequestDelegate next;


    public UnhandledExceptionMiddleware([NotNull] RequestDelegate next, [NotNull] ILog log)
    {
        this.next = next ?? throw new ArgumentNullException(nameof(next));
        this.log = (log ?? throw new ArgumentNullException(nameof(log))).ForContext<UnhandledExceptionMiddleware>();
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (CustomHttpException error)
        {
            log.Error(error, "An unhandled exception occurred during request processing. Response started = {ResponseHasStarted}.", context.Response.HasStarted);
            
            if (context.Response.HasStarted)
                throw;
            
            context.Response.Clear();
            context.Response.StatusCode = (int)error.CustomHttpCode;
            await context.Response.WriteAsync(JsonSerializer.Serialize(new { error.CustomError.Code, error.CustomError.Message }));
        }
    }
}
