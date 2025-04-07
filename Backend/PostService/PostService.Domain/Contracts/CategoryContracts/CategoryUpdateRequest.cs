namespace PostService.Domain.Contracts.CategoryContracts;

/// <summary>
/// DTO для обновления категории.
/// </summary>
/// <param name="Id">Идентификатор категории.</param>
/// <param name="Name">Название категории.</param>
public record CategoryUpdateRequest(Guid Id, string Name);