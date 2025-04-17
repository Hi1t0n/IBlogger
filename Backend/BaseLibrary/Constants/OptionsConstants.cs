using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Caching.Distributed;

namespace BaseLibrary.Constants;

public static class OptionsConstants
{
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