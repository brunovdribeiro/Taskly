using Infrastructure.Persistences.Posgres;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjectionUse
{
    public static async Task<IApplicationBuilder> UseInfrastructure(
        this IApplicationBuilder app
    )
    {
        using var scope = app.ApplicationServices.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        await dbContext.Database.MigrateAsync();
        
        return app;
    }
}