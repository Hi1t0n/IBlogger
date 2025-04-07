using PostService.Domain.Models;

namespace PostService.Domain.Contracts.CategoryContracts;

/// <summary>
/// DTO ответа для <see cref="Category"/>
/// </summary>
/// <param name="CategoryId">Идентификатор категории.</param>
/// <param name="CategoryName">Название категории.</param>
public record CategoryResponse(Guid CategoryId, string CategoryName);