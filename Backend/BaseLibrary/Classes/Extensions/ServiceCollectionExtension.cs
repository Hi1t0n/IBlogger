using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;

namespace BaseLibrary.Classes.Extensions;

/// <summary>
/// Методы расширения для <see cref="IServiceCollection"/>.
/// </summary>
public static class ServiceCollectionExtension
{
    /// <summary>
    /// Добавление логера и его конфигурация
    /// </summary>
    /// <param name="serviceCollection"><see cref="IServiceCollection"/>></param>
    /// <returns>Модифицированный <see cref="IServiceCollection"/></returns>
    public static IServiceCollection AddSerilogLogger(this IServiceCollection serviceCollection)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Debug(restrictedToMinimumLevel: LogEventLevel.Debug,
                outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u0}] {Message:lj} {Properties}{NewLine}{Exception}")
            .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Warning,
                outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u2}] {Message:lj} {Properties}{NewLine}{Exception}")
            .WriteTo.File(path: $"../logs/UserService_{DateTime.UtcNow:yyyyMMdd}_Log.txt",
                restrictedToMinimumLevel: LogEventLevel.Information,
                outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u2}] {Message:lj} {Properties}{NewLine}{Exception}",
                rollingInterval: RollingInterval.Day)
            .CreateBootstrapLogger();
    
        serviceCollection.AddSingleton(Log.Logger);
    
        return serviceCollection;
    }
}