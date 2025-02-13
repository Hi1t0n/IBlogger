namespace UserService.Domain.Contacts;

/// <summary>
/// Контракт для обновления пользователя.
/// </summary>
/// <param name="UserName">Имя пользователя.</param>
/// <param name="Email">Эл. Почта.</param>
/// <param name="PhoneNumber">Номер телефона.</param>
public record UpdateUserRequestContract(string UserName, string Email, string PhoneNumber);