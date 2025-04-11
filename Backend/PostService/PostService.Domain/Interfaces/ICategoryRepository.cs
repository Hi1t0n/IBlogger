using BaseLibrary.Interfaces;
using PostService.Domain.Models;

namespace PostService.Domain.Interfaces;

/// <summary>
/// Репозиторий <see cref="Category"/>.
/// </summary>
public interface ICategoryRepository : IRepository<Category>
{
    /// <summary>
    /// Получает все существующие категории из списка <paramref name="categories"/>.
    /// </summary>
    /// <param name="categories">Список всех категорий, которые переданы с клиента.</param>
    /// <returns>Список всех существующих категорий.</returns>
    public Task<List<Category>?> GetExistCategories(List<Guid> categories);
}