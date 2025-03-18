using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Caching.Distributed;
namespace UserService.Domain;

/// <summary>
/// Константы.
/// </summary>
public static class Constants
{
    /// <summary>
    /// Количество попыток для повторного подключения к БД.
    /// </summary>
    public static readonly int RetryOnFailure = 10;

    /// <summary>
    /// Redis.
    /// </summary>
    public static readonly string InstanceNameRedis = "UserService";

    /// <summary>
    /// Строка подключения к бд для Development.
    /// </summary>
    public static readonly string ConnectionStringDbConfiguration = "PostgreSQL";

    /// <summary>
    /// Строка подключения к бд для Production.
    /// </summary>
    public static readonly string ConnectionStringDbEnvironment = "CONNECTION_STRING_USER_SERVICE";

    /// <summary>
    /// Строка подключения к Redis для Development.
    /// </summary>
    public static readonly string ConnectionStringRedisConfiguration = "Redis";

    /// <summary>
    /// Строка подключения к Redis для Production.
    /// </summary>
    public static readonly string ConnectionStringRedisEnvironment = "CONNECTION_STRING_REDIS_USER_SERVICE";

    /// <summary>
    /// Настройка сериализации JSON.
    /// </summary>
    public static readonly JsonSerializerOptions? JsonSerializerOptions = new()
    {
        ReferenceHandler = ReferenceHandler.Preserve,
        WriteIndented = true
    };

    /// <summary>
    /// Настройка кэша.
    /// </summary>
    public static readonly DistributedCacheEntryOptions DistributedCacheEntryOptions = new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2),
    };
}