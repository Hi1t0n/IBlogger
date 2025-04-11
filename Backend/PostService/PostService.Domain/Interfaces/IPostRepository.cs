using BaseLibrary.Classes.Result;
using BaseLibrary.Interfaces;
using PostService.Domain.Models;

namespace PostService.Domain.Interfaces;

/// <summary>
/// Репозиторий <see cref="Post"/>.
/// </summary>
public interface IPostRepository : IRepository<Post>
{
    
    /// <summary>
    /// Получает все посты от определенного пользователя.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
    /// <returns>Список всех постов.</returns>
    Task<List<Post>?> GetPostsByUserId(Guid userId, CancellationToken cancellationToken);
}