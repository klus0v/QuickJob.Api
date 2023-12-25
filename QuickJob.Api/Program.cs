using QuickJob.Api.DI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSettings();
builder.Services.AddServiceAuthentication();
builder.Services.AddAuthMiddleware();
builder.Services.AddServiceSwaggerDocument();
builder.Services.AddServiceCors();
builder.Services.AddSystemServices();
builder.Services.AddExternalServices();
builder.Services.AddPostgresStorage();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();


var app = builder.Build();

//app.UseDeveloperExceptionPage()
app.UseUnhandledExceptionMiddleware();
app.UseSwaggerUi().UseOpenApi();
app.UseHttpsRedirection();
app.UseRouting();
app.UseServiceCors();
app.UseAuthorization();
app.UseAuthMiddleware();
app.MapControllers();

app.Run();
