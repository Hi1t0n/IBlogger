using Microsoft.Extensions.Hosting;
using Serilog;

namespace BaseLibrary.Classes.Extensions;

/// <summary>
/// Методы расширения для <see cref="IHostBuilder"/>.
/// </summary>
public static class HostBuilderExtension
{
    /// <summary>
    /// Используем логирование через Serilog с конфигурацией. 
    /// </summary>
    /// <param name="hostBuilder"><see cref="IHostBuilder"/>.</param>
    /// <returns>Модифицированный <see cref="IHostBuilder"/>.</returns>
    public static IHostBuilder UseSerilog(this IHostBuilder hostBuilder)
    {
        hostBuilder.UseSerilog((context, services, configuration) =>
        {
            configuration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext()
                .WriteTo.Console(
                    outputTemplate:
                    "[{Timestamp:HH:mm:ss} {Level:u0}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}")
                .WriteTo.File($"../logs/UserService_{DateTime.UtcNow:yyyyMMdd}_Log.txt", rollingInterval: RollingInterval.Day);
        });
        
        return hostBuilder;
    }
}