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
    public static void ApplyMigrations(this WebApplication webApplication)
    {
        using var scope = webApplication.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var pendingMigrations = context.Database.GetPendingMigrations();

        if (pendingMigrations.Any())
        {
            context.Database.Migrate();
            Console.WriteLine($"--> Migration apply");
        }
    }
}