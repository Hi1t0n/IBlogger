using BaseLibrary.Interfaces;
using UserService.Domain.Models;

namespace UserService.Domain.Interfaces;

/// <summary>
/// Интерфейс репозитория <see cref="User"/>
/// </summary>
public interface IUserRepository : IRepository<User>
{
    /// <summary>
    /// Восстановление пользователя по id
    /// </summary>
    /// <param name="id">Идентификатор пользователя</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    /// <returns>Восстановленный пользователь</returns>
    Task<User?> RestoreUserByIdAsync(Guid id, CancellationToken cancellationToken);
    /// <summary>
    /// Проверка существует ли email в БД
    /// </summary>
    /// <param name="email">Электронная почта</param>
    /// <returns>True, если email существует, иначе false</returns>
    Task<bool> ExistByEmail(string? email);
    /// <summary>
    /// Проверка существует ли номер телефона в БД
    /// </summary>
    /// <param name="phoneNumber">Электронная почта</param>
    /// <returns>True, если номер существует, иначе false</returns>
    Task<bool> ExistByPhoneNumber(string? phoneNumber);
    /// <summary>
    /// Проверка существует ли имя пользователя в БД
    /// </summary>
    /// <param name="userName">Электронная почта</param>
    /// <returns>True, если userName существует, иначе false</returns>
    Task<bool> ExistByUserName(string userName);
}