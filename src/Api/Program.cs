var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapTaskEndpoints();

app.Run();