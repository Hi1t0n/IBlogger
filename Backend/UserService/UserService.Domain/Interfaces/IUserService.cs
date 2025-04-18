using BaseLibrary.Classes.Result;
using BaseLibrary.Interfaces;
using UserService.Domain.Contacts;
using UserService.Domain.Models;

namespace UserService.Domain.Interfaces;

public interface IUserService
{
    public Task<Result<UserResponse?>> Add(AddUserRequest request, CancellationToken cancellationToken);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns>Результат получения коллекции всех сущностей.</returns>
    public Task<Result<List<UserResponse>?>> Get(CancellationToken cancellationToken);

    /// <summary>
    /// Получение сущности по <paramref name="id"/>.
    /// </summary>
    /// <param name="id">Идентификатор сущности.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
    /// <returns>Результат получения сущности с ее данными</returns>
    public Task<Result<UserResponse?>> GetById(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Удаление сущности по <paramref name="id"/>.
    /// </summary>
    /// <param name="id">Идентификатор сущности.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
    /// <returns>Результат удаления сущности с ее данными.</returns>
    public Task<Result<UserResponse?>> DeleteById(Guid id, CancellationToken cancellationToken);
    public Task<Result<UserResponse?>> UpdateById(UpdateUserRequest request, CancellationToken cancellationToken);
    public Task<Result<UserResponse?>> RestoreUserByIdAsync(Guid id, CancellationToken cancellationToken);
}