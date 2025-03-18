using BaseLibrary.Classes;
using UserService.Domain.Contacts;

namespace UserService.Domain.Models;

/// <summary>
/// Пользователь.
/// </summary>
public class User : BaseModel
{
    /// <summary>
    /// Имя пользователя.
    /// </summary>
    public string? UserName { get; set; } = string.Empty;
    
    /// <summary>
    /// Пароль.
    /// </summary>
    public string? Password { get; set; } = string.Empty;
    
    /// <summary>
    /// Идентификатор роли.
    /// </summary>
    public Guid? RoleId { get; set; }
    
    /// <summary>
    /// Эл.Почта.
    /// </summary>
    public string? Email { get; set; } = string.Empty;
    
    /// <summary>
    /// Подтверждение почты.
    /// </summary>
    public bool EmailConfirmed { get; set; } = false;
    
    /// <summary>
    /// Номер телефона.
    /// </summary>
    public string? PhoneNumber { get; set; } = string.Empty;
    
    /// <summary>
    /// Подтверждение номера телефона.
    /// </summary>
    public bool PhoneNumberConfirmed { get; set; } = false;
    
    /// <summary>
    /// Статус удаления.
    /// </summary>
    public bool IsDelete { get; set; } = false;
    
    /// <summary>
    /// Связывающее свойство с <see cref="Role"/>.
    /// </summary>
    public Role? Role { get; set; }

    /// <summary>
    /// Конвертация данных модели в ответ.
    /// </summary>
    /// <returns><see cref="UserResponseContract"/>.</returns>
    public UserResponseContract ToResponse()
    {
        return new UserResponseContract(Id, UserName!, Role!.RoleName!, Email, EmailConfirmed,
            PhoneNumber, PhoneNumberConfirmed, CreatedOn, ModifiedOn);
    }
}