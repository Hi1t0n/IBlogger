namespace BaseLibrary.Classes.Contracts;

/// <summary>
/// Контракт ответа.
/// </summary>
/// <param name="StatusCode">Статус код.</param>
/// <param name="Message">Сообщение.</param>
public record Response(int StatusCode,string Message);