using Microsoft.EntityFrameworkCore;
using PostService.Infrastructure.Context;

namespace PostService.Host.Extensions;

public static class WebApplicationExtensions
{
    public static async void ApplyMigrations(this WebApplication webApplication)
    {
        using var scope = webApplication.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var pendingMigrations = await context.Database.GetPendingMigrationsAsync();

        if (pendingMigrations.Any())
        {
            await context.Database.MigrateAsync();
            Console.WriteLine("--> Migration apply");
        }
    }
}