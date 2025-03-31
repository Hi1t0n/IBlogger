namespace PostService.Domain.Contracts;

/// <summary>
/// DTO добавления поста.
/// </summary>
/// <param name="Title">Заголовок.</param>
/// <param name="Content">Контент.</param>
/// <param name="UserId">Id автора поста.</param>
/// <param name="Categories">Список категорий поста в виде <see cref="Guid"/>.</param>
public record AddPostContract(string Title, string Content, Guid UserId, List<Guid> Categories);