using BaseLibrary.Classes.Result;
using PostService.Domain.Contracts.PostContracts;

namespace PostService.Domain.Interfaces;

/// <summary>
/// Сервис для постов.
/// </summary>
public interface IPostService
{
    /// <summary>
    /// Добавление поста.
    /// </summary>
    /// <param name="request">Данные для добавления <see cref="AddPostRequest"/>.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
    /// <returns>Результат добавления.</returns>
    public Task<Result<PostResponse?>> AddPost(AddPostRequest request, CancellationToken cancellationToken);
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns>Результат получения коллекции всех сущностей.</returns>
    public Task<Result<List<PostResponse>?>> GetAll(CancellationToken cancellationToken);

    /// <summary>
    /// Получение сущности по <paramref name="id"/>.
    /// </summary>
    /// <param name="id">Идентификатор сущности.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
    /// <returns>Результат получения сущности с ее данными</returns>
    public Task<Result<PostResponse?>> GetById(Guid id, CancellationToken cancellationToken);
    
    /// <summary>
    /// Получение всех постов у конкретного пользователя.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
    /// <returns>Список всех постов пользователя.</returns>
    public Task<Result<List<PostResponse>?>> GetPostsByUserId(Guid userId, CancellationToken cancellationToken);
    
    /// <summary>
    /// Обновление поста по идентификатору.
    /// </summary>
    /// <param name="request">Данные для обновления <see cref="PostUpdateRequest"/>.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
    /// <returns></returns>
    public Task<Result<PostResponse?>> UpdateById(PostUpdateRequest request, CancellationToken cancellationToken);
    
    /// <summary>
    /// Удаление сущности по <paramref name="id"/>.
    /// </summary>
    /// <param name="id">Идентификатор сущности.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
    /// <returns>Результат удаления сущности с ее данными.</returns>
    public Task<Result<PostResponse?>> DeleteById(Guid id, CancellationToken cancellationToken);
}