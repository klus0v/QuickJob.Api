using QuickJob.Api.DI;
using QuickJob.Api.Middlewares;
using UnhandledExceptionMiddleware = QuickJob.Api.Middlewares.UnhandledExceptionMiddleware;

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
builder.Services.AddPostgresStorage();



var app = builder.Build();

//app.UseDeveloperExceptionPage()
app.UseSwaggerUi3().UseOpenApi();
app.UseHttpsRedirection();
app.UseMiddleware<UnhandledExceptionMiddleware>();
app.UseMiddleware<UserAuthMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseCors(FrontSpecificOrigins);

app.Run();
