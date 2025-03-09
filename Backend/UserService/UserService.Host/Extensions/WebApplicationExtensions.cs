using Microsoft.EntityFrameworkCore;
using UserService.Infrastructure.Context;

namespace UserService.Host.Extensions;

/// <summary>
/// Предоставляет методы расширения для класса <see cref="WebApplication"/>.
/// </summary>
public static class WebApplicationExtensions
{
    /// <summary>
    /// Применение миграция БД.
    /// </summary>
    /// <param name="webApplication"><see cref="WebApplication"/></param>
    public static async void ApplyMigrations(this WebApplication webApplication)
    {
        using var scope = webApplication.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var pendingMigrations = await context.Database.GetPendingMigrationsAsync();

        if (pendingMigrations.Any())
        {
            await context.Database.MigrateAsync();
            Console.WriteLine($"--> Migration apply");
        }
    }
}