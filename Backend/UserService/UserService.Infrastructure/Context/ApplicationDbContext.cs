using System.Reflection;
using Microsoft.EntityFrameworkCore;
using UserService.Domain.Models;
using UserService.Infrastructure.Configurations;

namespace UserService.Infrastructure.Context;

/// <summary>
/// Контекст базы данных.
/// </summary>
public class ApplicationDbContext : DbContext
{
    /// <summary>
    /// Создает контекст базы данных.
    /// </summary>
    /// <param name="options">Настройки контекста.</param>
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }

    /// <summary>
    /// Конфигурация моделей(таблиц) в базе данных.
    /// </summary>
    /// <param name="modelBuilder"><see cref="ModelBuilder"/>.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
}