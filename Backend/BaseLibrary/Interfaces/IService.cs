using BaseLibrary.Classes.Result;

namespace BaseLibrary.Interfaces;

/// <summary>
/// Базовый сервис.
/// </summary>
/// <typeparam name="T">Тип данных сущности.</typeparam>
public interface IService<T>
{
    /// <summary>
    /// Получение всех сущностей.
    /// </summary>
    /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
    /// <returns>Результат получения коллекции всех сущностей.</returns>
    public Task<Result<IEnumerable<T>?>> Get(CancellationToken cancellationToken);
    
    /// <summary>
    /// Получение сущности по <paramref name="id"/>.
    /// </summary>
    /// <param name="id">Идентификатор сущности.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
    /// <returns>Результат получения сущности с ее данными</returns>
    public Task<Result<T?>> GetById(Guid id, CancellationToken cancellationToken);
    
    /// <summary>
    /// Удаление сущности по <paramref name="id"/>.
    /// </summary>
    /// <param name="id">Идентификатор сущности.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
    /// <returns>Результат удаления сущности с ее данными.</returns>
    public Task<Result<T?>> DeleteById(Guid id, CancellationToken cancellationToken);
}