using Application;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactApp", builder =>
        builder.WithOrigins("http://localhost:3000")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

var app = builder.Build();

app.UseCors("ReactApp");

// Serve static files
app.UseDefaultFiles();
app.UseStaticFiles();

app.MapFallbackToFile("index.html");

app.UseOpenApi();
app.UseSwaggerUi();

await app.UseInfrastructure();

app.UseCors("ReactApp");

app.MapTaskEndpoints();
app.MapUserEndpoints();

app.Run();