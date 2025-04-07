using PostService.Domain.Contracts.CategoryContracts;
using PostService.Domain.Models;

namespace PostService.Domain.Contracts.PostContracts;

/// <summary>
/// DTO ответа для <see cref="Post"/>
/// </summary>
/// <param name="PostId">Идентификатор поста.</param>
/// <param name="Title">Заголовок.</param>
/// <param name="Content">Контент поста.</param>
/// <param name="UserId">Идентификатор пользователя, который добавил пост.</param>
/// <param name="Categories">Коллекция всех категорий</param>
public record PostResponse(Guid PostId, string Title, string Content, Guid UserId, List<CategoryResponse> Categories);