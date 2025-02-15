using Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseOpenApi();
app.UseSwaggerUi(); 

app.MapTaskEndpoints();

app.Run();