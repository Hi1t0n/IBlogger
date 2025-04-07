namespace PostService.Domain.Contracts.PostContracts;

/// <summary>
/// DTO добавления поста.
/// </summary>
/// <param name="Title">Заголовок.</param>
/// <param name="Content">Контент.</param>
/// <param name="UserId">Id автора поста.</param>
/// <param name="Categories">Список категорий поста в виде <see cref="Guid"/>.</param>
public record AddPostRequest(string Title, string Content, Guid UserId, List<Guid> Categories);