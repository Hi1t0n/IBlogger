namespace PostService.Domain.Constants;

/// <summary>
/// Константы для конфигурации Redis.
/// </summary>
public static class RedisConfig
{
    /// <summary>
    /// Название строки подключения Redis в конфигурации.
    /// </summary>
    public const string RedisConnectionStringConfigurationName = "CONNECTION_STRING_REDIS_POST_SERVICE";
    
    /// <summary>
    /// Инстанс имя для каждой записи.
    /// </summary>
    public const string InstanceNameRedis = "PostService";
}