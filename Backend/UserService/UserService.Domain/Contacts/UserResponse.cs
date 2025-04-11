namespace UserService.Domain.Contacts;

/// <summary>
/// Контракт для ответа данных пользователя.
/// </summary>
/// <param name="Id">Идентификатор.</param>
/// <param name="UserName">Имя пользователя.</param>
/// <param name="RoleName">Название роли.</param>
/// <param name="Email">Эл.Почта.</param>
/// <param name="EmailConfirmed">Статус подтверждения почты.</param>
/// <param name="PhoneNumber">Номер телефона.</param>
/// <param name="PhoneNumberConfirmed">Статус подтверждения номера телефона.</param>
/// <param name="CreatedOn">Дата создания.</param>
/// <param name="ModifiedOn">Дата изменения.</param>
public record UserResponse(
    Guid Id, 
    string UserName,
    string RoleName,
    string? Email,
    bool? EmailConfirmed,
    string? PhoneNumber,
    bool? PhoneNumberConfirmed,
    DateTime CreatedOn, 
    DateTime ModifiedOn
    );