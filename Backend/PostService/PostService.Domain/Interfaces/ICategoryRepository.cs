using PostService.Domain.Contracts;
using PostService.Domain.Models;

namespace PostService.Domain.Interfaces;

/// <summary>
/// Репозиторий <see cref="Category"/>.
/// </summary>
public interface ICategoryRepository
{
    /// <summary>
    /// Добавление категории.
    /// </summary>
    /// <param name="category">Данные категории для добавления.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    /// <returns>Данные добавленного поста.</returns>
    public Task<Category?> Add(Category category, CancellationToken cancellationToken);
    
    /// <summary>
    /// Получение всех категорий.
    /// </summary>
    /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
    /// <returns>Список всех категорий.</returns>
    public Task<List<Category>?> Get(CancellationToken cancellationToken);
    
    /// <summary>
    /// Получает все существующие категории из списка <paramref name="categories"/>.
    /// </summary>
    /// <param name="categories">Список всех категорий, которые переданы с клиента.</param>
    /// <returns>Список всех существующих категорий.</returns>
    public Task<List<Category>?> GetExistCategories(List<Guid> categories);

    /// <summary>
    /// Получение категории по <paramref name="id"/>.
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
    /// <returns>Категория.</returns>
    public Task<Category?> GetById(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Обновление категории.
    /// </summary>
    /// <param name="updateData">Данные категории для обновления.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
    /// <returns>Обновленная категория.</returns>
    public Task<Category?> UpdateById(Category updateData, CancellationToken cancellationToken);

    /// <summary>
    /// Удаление категории по <paramref name="id"/>.
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
    /// <returns>Данные удаленной категории.</returns>
    public Task<Category?> DeleteById(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Считает количество постов у каждой категории.
    /// </summary>
    /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
    /// <returns>Список категорий с количеством постов.</returns>
    public Task<List<CategoryPostCount>> CountPostsForCategories(CancellationToken cancellationToken);
}