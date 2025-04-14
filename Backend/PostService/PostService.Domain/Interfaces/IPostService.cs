using BaseLibrary.Classes.Result;
using BaseLibrary.Interfaces;
using PostService.Domain.Contracts.PostContracts;
using PostService.Domain.Models;

namespace PostService.Domain.Interfaces;

/// <summary>
/// Сервис для постов.
/// </summary>
public interface IPostService : IService<Post>
{
    /// <summary>
    /// Добавление поста.
    /// </summary>
    /// <param name="request">Данные для добавления <see cref="AddPostRequest"/>.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
    /// <returns>Результат добавления.</returns>
    public Task<Result<Post?>> AddPost(AddPostRequest request, CancellationToken cancellationToken);
    
    /// <summary>
    /// Получение всех постов у конкретного пользователя.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
    /// <returns>Список всех постов пользователя.</returns>
    public Task<Result<List<Post>?>> GetPostsByUserId(Guid userId, CancellationToken cancellationToken);
    
    /// <summary>
    /// Обновление поста по идентификатору.
    /// </summary>
    /// <param name="request">Данные для обновления <see cref="PostUpdateRequest"/>.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
    /// <returns></returns>
    public Task<Result<Post?>> UpdateById(PostUpdateRequest request, CancellationToken cancellationToken);
}