namespace PostService.Domain.Contracts.PostContracts;

/// <summary>
/// DTO для обновления поста.
/// </summary>
/// <param name="Id">Идентификатор поста.</param>
/// <param name="Title">Новый заголовок</param>
/// <param name="Content">Новое содержимое поста.</param>
/// <param name="Categories">Список категорий.</param>
public record PostUpdateRequest(Guid Id, string Title, string Content, List<Guid> Categories);