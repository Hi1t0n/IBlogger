using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using UserService.Domain.Interfaces;
using UserService.Infrastructure.Context;
using UserService.Infrastructure.Repository;
using Constants = UserService.Domain.Constants;

namespace UserService.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Добавление бизнес логики
    /// </summary>
    /// <param name="serviceCollection"><see cref="ServiceCollection"/></param>
    /// <param name="connectionStringDb">Строка подключения к БД</param>
    /// <param name="connectionStringRedis">Строка подключения к Redis</param>
    /// <returns>Модифицированный <see cref="ServiceCollection"/></returns>
    public static IServiceCollection AddBusinessLogic(this IServiceCollection serviceCollection, string connectionStringDb, string connectionStringRedis)
    {
        serviceCollection.AddServices();
        serviceCollection.AddDataBase(connectionStringDb);
        serviceCollection.AddRedis(connectionStringRedis);
        serviceCollection.AddSerilogLogger();
        return serviceCollection;
    }

    /// <summary>
    /// Добавление базы данных
    /// </summary>
    /// <param name="serviceCollection"><see cref="ServiceCollection"/></param>
    /// <param name="connectionString">Строка подключения</param>
    /// <returns>Модифицированный <see cref="ServiceCollection"/></returns>
    private static IServiceCollection AddDataBase(this IServiceCollection serviceCollection, string connectionString)
    {
        serviceCollection.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString, 
                builder =>
                {
                    builder.EnableRetryOnFailure(Constants.RetryOnFailure);
                }));

        return serviceCollection;
    }

    /// <summary>
    /// Добавление Redis
    /// </summary>
    /// <param name="serviceCollection"><see cref="ServiceCollection"/></param>
    /// <param name="connectionString">Строка подключения</param>
    /// <returns>Модифицированный <see cref="ServiceCollection"/></returns>
    private static IServiceCollection AddRedis(this IServiceCollection serviceCollection, string connectionString)
    {
        serviceCollection.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = connectionString;
            options.InstanceName = Constants.InstanceNameRedis;
        });

        return serviceCollection;
    }
    
    /// <summary>
    /// Добавление сервисов
    /// </summary>
    /// <param name="serviceCollection"><see cref="ServiceCollection"/></param>
    /// <returns>Модифицированный <see cref="ServiceCollection"/></returns>
    private static IServiceCollection AddServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IUserRepository, UserRepository>();

        return serviceCollection;
    }
    
    /// <summary>
    /// Добавление логера и его конфигурация
    /// </summary>
    /// <param name="serviceCollection"><see cref="IServiceCollection"/>></param>
    /// <returns>Модифицированный <see cref="IServiceCollection"/></returns>
    private static IServiceCollection AddSerilogLogger(this IServiceCollection serviceCollection)
    {
        
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Debug(restrictedToMinimumLevel: LogEventLevel.Debug,
                outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u2}] {Message:lj} {Properties}{NewLine}{Exception}")
            .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Warning,
                outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u2}] {Message:lj} {Properties}{NewLine}{Exception}")
            .WriteTo.File(path: $"../logs/UserService_{DateTime.UtcNow:yyyyMMdd}_Log.txt",
                restrictedToMinimumLevel: LogEventLevel.Information,
                outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u2}] {Message:lj} {Properties}{NewLine}{Exception}",
                rollingInterval: RollingInterval.Day)
            .CreateLogger();

        serviceCollection.AddSingleton(Log.Logger);
        
        return serviceCollection;
    }
}