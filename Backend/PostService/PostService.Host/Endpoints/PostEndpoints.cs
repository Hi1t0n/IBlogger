using BaseLibrary.Classes.Contracts;
using PostService.Domain.Contracts;
using PostService.Domain.Extensions;
using PostService.Domain.Interfaces;
using PostService.Domain.Models;

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
        
        return webApplication;
    }

    /// <summary>
    /// Добавление поста. 
    /// </summary>
    /// <param name="contract">Данные поста.</param>
    /// <param name="repository"><see cref="IPostRepository"/>.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
    private static async Task<IResult> AddPost(AddPostContract contract,
        IPostRepository repository, CancellationToken cancellationToken)
    {
        var post = contract.ToModel();

        await repository.Create(post, cancellationToken);

        return Results.Ok(new Response(StatusCodes.Status200OK, String.Empty));
    }
}