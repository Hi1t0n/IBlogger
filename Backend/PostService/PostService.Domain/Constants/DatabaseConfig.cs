namespace PostService.Domain.Constants;

/// <summary>
/// Константы для конфигурации базы данных.
/// </summary>
public static class DatabaseConfig
{
    /// <summary>
    /// Название строки подключения в конфигурации.
    /// </summary>
    public const string DataBaseConnectionStringConfigurationName = "CONNECTION_STRING_POST_SERVICE";
    
    /// <summary>
    /// Количество повторных подключений при неудаче.
    /// </summary>
    public static readonly int RetryOnFailure = 10;
}