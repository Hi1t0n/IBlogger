namespace UserService.Domain.Contacts;

/// <summary>
/// Контракт ответа.
/// </summary>
/// <param name="StatusCode">Статус код.</param>
/// <param name="Message">Сообщение.</param>
public record Response(int StatusCode,string Message);