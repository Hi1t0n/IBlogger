namespace UserService.Domain.Constants;

/// <summary>
/// Константы.
/// </summary>
public static class DatabasesConfigurationConstants
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

    
}