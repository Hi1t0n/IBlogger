using BaseLibrary.Classes.Result;
using UserService.Domain.Contacts;

namespace UserService.Domain.Interfaces;

public interface IUserService
{
    public Task<Result<UserResponse?>> AddUserAsync(AddUserRequest request, CancellationToken cancellationToken);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns>Результат получения коллекции всех сущностей.</returns>
    public Task<Result<List<UserResponse>?>> GetUsersAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Получение сущности по <paramref name="id"/>.
    /// </summary>
    /// <param name="id">Идентификатор сущности.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
    /// <returns>Результат получения сущности с ее данными</returns>
    public Task<Result<UserResponse?>> GetUserByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Удаление сущности по <paramref name="id"/>.
    /// </summary>
    /// <param name="id">Идентификатор сущности.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
    /// <returns>Результат удаления сущности с ее данными.</returns>
    public Task<Result<UserResponse?>> DeleteUserByIdAsync(Guid id, CancellationToken cancellationToken);
    
    /// <summary>
    /// Обновление данных пользователя по идентификатору.
    /// </summary>
    /// <param name="request">Данные из запроса.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
    /// <returns>Результат обновления с данными пользователя.</returns>
    public Task<Result<UserResponse?>> UpdateUserByIdAsync(UpdateUserRequest request, CancellationToken cancellationToken);
    
    /// <summary>
    /// Восстановление пользователя по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор пользователя.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
    /// <returns>Результат восстановления с данными пользователя.</returns>
    public Task<Result<UserResponse?>> RestoreUserByIdAsync(Guid id, CancellationToken cancellationToken);
}