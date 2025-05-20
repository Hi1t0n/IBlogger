using PostService.Domain.Models;

namespace PostService.Domain.Interfaces;

/// <summary>
/// Репозиторий <see cref="Post"/>.
/// </summary>
public interface IPostRepository
{
    /// <summary>
    /// Добавление поста.
    /// </summary>
    /// <param name="post">Данные поста для добавления.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
    /// <returns>Данные добавленного поста.</returns>
    public Task<Post?> Add(Post post, CancellationToken cancellationToken);

    /// <summary>
    /// Получение всех постов.
    /// </summary>
    /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
    /// <returns>Список всех постов.</returns>
    public Task<List<Post>?> Get(CancellationToken cancellationToken);

    /// <summary>
    /// Получение поста по <paramref name="id"/>.
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
    /// <returns>Пост.</returns>
    public Task<Post?> GetById(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Обновление поста.
    /// </summary>
    /// <param name="updateData">Данные поста для обновления.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
    /// <returns>Обновленный пост.</returns>
    public Task<Post?> UpdateById(Post updateData, CancellationToken cancellationToken);

    /// <summary>
    /// Удаление поста по <paramref name="id"/>.
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
    /// <returns>Данные удаленного поста.</returns>
    public Task<Post?> DeleteById(Guid id, CancellationToken cancellationToken);
    
    /// <summary>
    /// Получает все посты от определенного пользователя.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
    /// <returns>Список всех постов.</returns>
    Task<List<Post>?> GetPostsByUserId(Guid userId, CancellationToken cancellationToken);
}