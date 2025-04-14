namespace BaseLibrary.Constants;

/// <summary>
/// Шаблоны для ключей Redis.
/// </summary>
public static class RedisKeysConstants
{
    /// <summary>
    /// Ключ для всех Entity, nameof(EntityType).
    /// </summary>
    public static readonly string AllEntityKeyTemplate = "{0}s";
    
    /// <summary>
    /// Ключ для конкретной записи Entity.
    /// {0} - Id Entity, {1} - nameof(EntityType)
    /// </summary>
    public static readonly string EntityWithIdKeyTemplate = "{0}_{1}";
}