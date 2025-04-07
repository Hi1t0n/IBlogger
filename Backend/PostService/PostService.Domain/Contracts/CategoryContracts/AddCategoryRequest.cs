namespace PostService.Domain.Contracts.CategoryContracts;

/// <summary>
/// DTO добавления категории.
/// </summary>
/// <param name="CategoryName">Название категории.</param>
public record AddCategoryRequest(string CategoryName);