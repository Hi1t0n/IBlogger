namespace UserService.Domain.Contacts;

/// <summary>
/// Контракт запроса для добавления пользователя.
/// </summary>
/// <param name="UserName">Имя пользователя.</param>
/// <param name="Password">Пароль.</param>
/// <param name="Email">Эл. Почта.</param>
/// <param name="PhoneNumber">Номер телефона.</param>
public record AddUserRequestContract(string UserName, string Password, string? Email, string? PhoneNumber);