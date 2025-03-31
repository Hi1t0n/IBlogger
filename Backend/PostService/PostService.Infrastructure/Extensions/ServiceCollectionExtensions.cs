using BaseLibrary.Classes.Extensions;
using Microsoft.Extensions.DependencyInjection;
using PostService.Domain.Constants;
using PostService.Domain.Interfaces;
using PostService.Infrastructure.Context;
using PostService.Infrastructure.Repository;

namespace PostService.Infrastructure.Extensions;

/// <summary>
/// Методы расширения для <see cref="IServiceCollection"/>.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Добавление бизнес логики.
    /// </summary>
    /// <param name="serviceCollection"><see cref="IServiceCollection"/>.</param>
    /// <param name="connectionStringDataBase">Строка подключение к базе данных</param>
    /// <param name="connectionStringRedis">Строка подключения к Redis.</param>
    /// <returns>Модифицированный <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddBusinessLogic(this IServiceCollection serviceCollection,
        string connectionStringDataBase, string connectionStringRedis)
    {
        serviceCollection.AddDatabase(connectionStringDataBase);
        serviceCollection.AddRedis(connectionStringRedis);
        serviceCollection.AddService();
        serviceCollection.AddSerilogLogger();
        
        return serviceCollection;
    }

    /// <summary>
    /// Добавление базы данных.
    /// </summary>
    /// <param name="serviceCollection"><see cref="IServiceCollection"/>.</param>
    /// <param name="connectionString">Строка подключения.</param>
    /// <returns>Модифицированный <see cref="IServiceCollection"/>.</returns>
    private static IServiceCollection AddDatabase(this IServiceCollection serviceCollection, string connectionString)
    {
        serviceCollection.AddNpgsql<ApplicationDbContext>(connectionString, builder =>
        {
            builder.EnableRetryOnFailure(DatabaseConfig.RetryOnFailure);
        } );

        return serviceCollection;
    }

    /// <summary>
    /// Добавление Redis.
    /// </summary>
    /// <param name="serviceCollection"><see cref="IServiceCollection"/>.</param>
    /// <param name="connectionString">Строка подключения.</param>
    /// <returns>Модифицированный <see cref="IServiceCollection"/>.</returns>
    private static IServiceCollection AddRedis(this IServiceCollection serviceCollection, string connectionString)
    {
        serviceCollection.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = connectionString;
            options.InstanceName = RedisConfig.InstanceNameRedis;
        });

        return serviceCollection;
    }

    /// <summary>
    /// Добавление сервисов в DI.
    /// </summary>
    /// <param name="serviceCollection"><see cref="IServiceCollection"/>.</param>
    /// <returns>Модифицированный <see cref="IServiceCollection"/>.</returns>
    private static IServiceCollection AddService(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IPostRepository, PostRepository>();
        
        return serviceCollection;
    }
}