using BaseLibrary.Classes.Contracts;
using BaseLibrary.Classes.Result;
using PostService.Domain.Contracts.PostContracts;
using PostService.Domain.Interfaces;
using PostService.Domain.Models;
using PostService.Infrastructure.Mappings;

namespace PostService.Host.Endpoints;

/// <summary>
/// Эндпоинты для <see cref="Post"/>.
/// </summary>
public static class PostEndpoints
{
    /// <summary>
    /// Добавление эндпоинтов.
    /// </summary>
    /// <param name="webApplication"><see cref="WebApplication"/>.</param>
    /// <returns>Модифицированный <see cref="WebApplication"/>.</returns>
    public static WebApplication AddPostEndpoints(this WebApplication webApplication)
    {
        var mapGroup = webApplication.MapGroup("/api/posts/");

        mapGroup.MapPost("/", AddPost);
        mapGroup.MapGet("/", GetPosts);
        mapGroup.MapGet("/{id:Guid}", GetPostById);
        mapGroup.MapGet("/users/{id:Guid}", GetPostsByUserId);
        mapGroup.MapPut("/", UpdatePostById);
        mapGroup.MapDelete("/", DeletePostById);

        return webApplication;
    }

    /// <summary>
    /// Добавление поста. 
    /// </summary>
    /// <param name="request">Данные поста.</param>
    /// <param name="postService"><see cref="IPostService"/>.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
    /// <returns>Результат добавления и <see cref="Response{T}"/>.</returns>
    private static async Task<IResult> AddPost(AddPostRequest request,
        IPostService postService,
        CancellationToken cancellationToken)
    {
        await postService.Add(request, cancellationToken);

        return Results.Ok(new Response(StatusCodes.Status200OK, String.Empty));
    }

    /// <summary>
    /// Получение всех постов.
    /// </summary>
    /// <param name="postService"><see cref="IPostService"/>.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
    /// <returns>Коллекция постов.</returns>
    private static async Task<IResult> GetPosts(IPostService postService, CancellationToken cancellationToken)
    {
        var result = await postService.Get(cancellationToken);

        return Results.Ok(
            new Response<IEnumerable<PostResponse>>(StatusCodes.Status200OK, result.Value!.ToResponse()));
    }

    /// <summary>
    /// Получение поста по <paramref name="id"/>.
    /// </summary>
    /// <param name="id">Идентификатор поста.</param>
    /// <param name="postService"><see cref="IPostService"/>.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
    /// <returns>Результат с <see cref="Response{T}"/>.</returns>
    private static async Task<IResult> GetPostById(Guid id,
        IPostService postService, CancellationToken cancellationToken)
    {
        var result = await postService.GetById(id, cancellationToken);

        if (!result.IsSuccess)
        {
            switch (result.ResultType)
            {
                case ResultType.NotFound:
                    return Results.NotFound(new Response(StatusCodes.Status404NotFound, result.Message));
            }
        }

        return Results.Ok(new Response<PostResponse>(StatusCodes.Status200OK, result.Value!.ToResponse()));
    }

    /// <summary>
    /// Получение всех постов по <paramref name="id"/> пользователя.
    /// </summary>
    /// <param name="id">Идентификатор пользователя.</param>
    /// <param name="postService"><see cref="IPostService"/>.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
    /// <returns>Результат с <see cref="Response{T}"/>.</returns>
    private static async Task<IResult> GetPostsByUserId(Guid id, IPostService postService,
        CancellationToken cancellationToken)
    {
        var posts = await postService.GetPostsByUserId(id, cancellationToken);

        return Results.Ok(
            new Response<IEnumerable<PostResponse>>(StatusCodes.Status200OK, posts.Value.ToResponse()));
    }

    /// <summary>
    /// Обновление поста.
    /// </summary>
    /// <param name="request">Данные для обновления поста.</param>
    /// <param name="postService"><see cref="IPostRepository"/>.</param>
    /// <param name="categoryRepository"><see cref="ICategoryRepository"/>.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
    /// <returns>Результат с <see cref="Response{T}"/>.</returns>
    private static async Task<IResult> UpdatePostById(PostUpdateRequest request,
        IPostService postService, ICategoryRepository categoryRepository, CancellationToken cancellationToken)
    {
        var result = await postService.UpdateById(request,cancellationToken);

        if (!result.IsSuccess)
        {
            switch (result.ResultType)
            {
                case ResultType.NotFound:
                    return Results.NotFound(new Response(StatusCodes.Status404NotFound, result.Message));
            }
        }

        return Results.Ok(new Response(StatusCodes.Status200OK, string.Empty));
    }

    /// <summary>
    /// Удаление поста по <paramref name="id"/>.
    /// </summary>
    /// <param name="id">Идентификатор поста.</param>
    /// <param name="postService"><see cref="IPostService"/>.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
    /// <returns>Результат с <see cref="Response{T}"/>.</returns>
    private static async Task<IResult> DeletePostById(Guid id,
        IPostService postService, CancellationToken cancellationToken)
    {
        var result = await postService.DeleteById(id, cancellationToken);

        if (!result.IsSuccess)
        {
            switch (result.ResultType)
            {
                case ResultType.NotFound:
                    return Results.NotFound(new Response(StatusCodes.Status404NotFound, result.Message));
            }
        }

        return Results.Ok(new Response(StatusCodes.Status200OK, string.Empty));
    }
}