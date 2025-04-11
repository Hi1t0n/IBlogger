using BaseLibrary.Classes.Result;

namespace BaseLibrary.Interfaces;

/// <summary>
/// Базовый репозиторий.
/// </summary>
public interface IRepository<T>
{
    /// <summary>
    /// Создание сущности.
    /// </summary>
    /// <param name="entity">Данные сущности.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
    /// <typeparam name="T">Тип сущности.</typeparam>
    /// <returns>Созданная сущность.</returns>
    public Task<T?> Add(T entity, CancellationToken cancellationToken);
    
    /// <summary>
    /// Получение всех сущностей.
    /// </summary>
    /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
    /// <typeparam name="T">Тип сущности.</typeparam>
    /// <returns>Коллекция сущностей.</returns>
    public Task<IEnumerable<T>?> Get(CancellationToken cancellationToken);

    /// <summary>
    /// Получение сущности по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор сущности.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
    /// <typeparam name="T">Тип данных сущности.</typeparam>
    /// <returns>Сущность.</returns>
    public Task<T?> GetById(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Обновление сущности по Id.
    /// </summary>
    /// <param name="updateData">Данные для обновления.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
    /// <typeparam name="T">Тип данных сущности, которую обновляем.</typeparam>
    /// <returns>Сущность с обновленными данными.</returns>
    public Task<T?> UpdateById(T updateData, CancellationToken cancellationToken);

    /// <summary>
    /// Удаление сущности по Id.
    /// </summary>
    /// <param name="id">Идентификатор сущности.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
    /// <typeparam name="T">Тип данных сущности.</typeparam>
    /// <returns>Удаленная сущность.</returns>
    public Task<T?> DeleteById(Guid id, CancellationToken cancellationToken);
}