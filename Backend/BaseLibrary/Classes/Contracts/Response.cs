namespace BaseLibrary.Classes.Contracts;

/// <summary>
/// Контракт ответа.
/// </summary>
/// <param name="StatusCode">Статус код.</param>
/// <param name="Message">Сообщение.</param>
public record Response(int StatusCode, string? Message);

/// <summary>
/// Контракт ответа с данными.
/// </summary>
/// <param name="StatusCode">Статус код.</param>
/// <param name="Data"></param>
/// <typeparam name="T"></typeparam>
public record Response<T>(int StatusСode, T Data);