using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using QuickJob.Api.Authentication;
using QuickJob.Api.DI;

const string FrontSpecificOrigins = "_frontSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSettings();
builder.Services.AddSystemServices();
builder.Services.AddExternalServices();
builder.Services.AddServiceCors();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddServiceAuthentication();
builder.Services.AddServiceSwaggerDocument();

var app = builder.Build();

app.UseDeveloperExceptionPage().UseSwaggerUi3().UseOpenApi();
app.UseHttpsRedirection();
app.UseExceptionHandler(ConfigureMiddleware());
app.UseMiddleware<UserAuthMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseCors(FrontSpecificOrigins);
    

app.Run();




//todo костыль

Action<IApplicationBuilder> ConfigureMiddleware() => errorApp =>
{
    errorApp.Run(async context =>
    {
        var exception = context.Features.Get<IExceptionHandlerFeature>();
        if (exception != null)
        {
            var errorMessage = new { exception.Error.Message };
            var jsonError = JsonSerializer.Serialize(errorMessage);
            var statusCode = exception.Error.HResult is > 100 and < 503 ? exception.Error.HResult : 500;
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(jsonError);
        }
    });
};
