using System.Security.Cryptography.Xml;
using PostService.Domain.Contracts.CategoryContracts;
using PostService.Domain.Models;

namespace PostService.Infrastructure.Mappings;

/// <summary>
/// Маппинг для <see cref="Category"/>.
/// </summary>
public static class CategoryMapping
{
    /// <summary>
    /// DTO <see cref="AddCategoryRequest"/> в <see cref="Category"/>.
    /// </summary>
    /// <param name="request">DTO с данными для добавления.</param>
    /// <returns>Модель <see cref="Category"/>.</returns>
    public static Category ToModel(this AddCategoryRequest request)
    {
        return new Category
        {
            Id = Guid.NewGuid(),
            Name = request.CategoryName
        };
    }

    /// <summary>
    /// DTO <see cref="CategoryUpdateRequest"/> в <see cref="Category"/>.
    /// </summary>
    /// <param name="request">DTO с данными для обновления.</param>
    /// <returns>Модель <see cref="Category"/>.</returns>
    public static Category ToModel(this CategoryUpdateRequest request)
    {
        return new Category
        {
            Id = request.Id,
            Name = request.Name
        };
    }

    /// <summary>
    /// <see cref="Category"/> в DTO <see cref="CategoryResponse"/>.
    /// </summary>
    /// <param name="category">Объект типа <see cref="Category"/>.</param>
    /// <returns><see cref="CategoryResponse"/>.</returns>
    public static CategoryResponse ToResponse(this Category category)
    {
        return new CategoryResponse(category.Id, category.Name);
    }

    /// <summary>
    /// Коллекция <see cref="Category"/> в коллекцию DTO <see cref="CategoryResponse"/>.
    /// </summary>
    /// <param name="categories">Коллекция с объектами <see cref="Category"/>.</param>
    /// <returns>Коллекция с объектами <see cref="CategoryResponse"/>.</returns>
    public static IEnumerable<CategoryResponse> ToResponse(IEnumerable<Category> categories)
    {
        return categories
            .Select(x => x.ToResponse())
            .ToList();
    }
}