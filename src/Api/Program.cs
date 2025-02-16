using Application;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseOpenApi();
app.UseSwaggerUi();

await app.UseInfrastructure();

app.MapTaskEndpoints()
    .MapUserEndpoints();

app.Run();