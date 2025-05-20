using UserService.Domain.Models;

namespace UserService.Domain.Interfaces;

/// <summary>
/// Интерфейс репозитория <see cref="User"/>
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Добавление пользователя.
    /// </summary>
    /// <param name="user">Данные пользователя <see cref="User"/>.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
    /// <returns>Данные добавленного пользователя.</returns>
    public Task<User?> AddUserAsync(User user, CancellationToken cancellationToken);

    /// <summary>
    /// Получение данных пользователя по id.
    /// </summary>
    /// <param name="id">Идентификатор пользователя.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
    /// <returns>Данные пользователя.</returns>
    public Task<User?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Получение пользователей.
    /// </summary>
    /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
    /// <returns>Список всех пользователей.</returns>
    public Task<List<User>?> GetUsersAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Обновление данных пользователя по идентификатору.
    /// </summary>
    /// <param name="user">Новые данные пользователя.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
    /// <returns>Количество обновленных данных.</returns>
    public Task<int?> UpdateUserById(User user, CancellationToken cancellationToken);

    /// <summary>
    /// Удаление пользователя по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор пользователя.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
    /// <returns>Количество удаленных пользователей, при восстановлении "1" иначе "0".</returns>
    public Task<int?> DeleteUserById(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Восстановление пользователя по id
    /// </summary>
    /// <param name="id">Идентификатор пользователя</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    /// <returns>Количество восстановленных пользователей, при восстановлении "1" иначе "0".</returns>
    Task<int?> RestoreUserByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Проверка существует ли email в БД
    /// </summary>
    /// <param name="email">Электронная почта</param>
    /// <returns>True, если email существует, иначе false</returns>
    Task<bool> ExistByEmailAsync(string? email);

    /// <summary>
    /// Проверка существует ли номер телефона в БД
    /// </summary>
    /// <param name="phoneNumber">Электронная почта</param>
    /// <returns>True, если номер существует, иначе false</returns>
    Task<bool> ExistByPhoneNumberAsync(string? phoneNumber);

    /// <summary>
    /// Проверка существует ли имя пользователя в БД
    /// </summary>
    /// <param name="userName">Электронная почта</param>
    /// <returns>True, если userName существует, иначе false</returns>
    Task<bool> ExistByUserNameAsync(string userName);
}