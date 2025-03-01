using Api.Tasks;
using Api.Users;
using Application;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddHealthChecks();

var app = builder.Build();

// Configure the HTTP request pipeline
app.UseDefaultFiles();
app.UseStaticFiles();

// Map API endpoints under /api
app.MapGroup("/api")
    .MapTaskEndpoints()
    .MapUserEndpoints();

app.UseOpenApi();
app.UseSwaggerUi();

if (!args.Contains("--no-db"))
{
    await app.UseInfrastructure();
}

app.UseHealthChecks("/health");

// Serve Next.js app for non-API routes
app.MapFallbackToFile("index.html");

app.Run();