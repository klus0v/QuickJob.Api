using QuickJob.Api.Consumer.DI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSettings();
builder.Services.AddExternalServices();
builder.Services.AddSystemServices();
builder.Services.AddRabbitMq();

var app = builder.Build();

app.Run();