using PostService.Domain.Contracts.CategoryContracts;
using PostService.Domain.Contracts.PostContracts;
using PostService.Domain.Models;

namespace PostService.Infrastructure.Mappings;

/// <summary>
/// Маппинг для <see cref="Post"/>.
/// </summary>
public static class PostMapping
{
    /// <summary>
    /// <see cref="AddPostRequest"/> в <see cref="Post"/>.
    /// </summary>
    /// <param name="request">Данные в DTO для добавления поста.</param>
    /// <param name="existingCategory">Существующие категории.</param>
    /// <returns>Модель <see cref="Post"/>.</returns>
    public static Post ToModel(this AddPostRequest request, List<Category> existingCategory)
    {
        var postId = Guid.NewGuid();

        var post = new Post()
        {
            Id = postId,
            Title = request.Title,
            UserId = request.UserId,
            Content = request.Content,
            CreatedOn = DateTime.UtcNow,
            ModifiedOn = DateTime.UtcNow,
            PostCategories = existingCategory.Select(category => new PostCategory
            {
                PostId = postId,
                CategoryId = category.Id,
                Category = category
            }).ToList()
        };

        return post;
    }

    /// <summary>
    /// <see cref="PostUpdateRequest"/> в <see cref="Post"/>.
    /// </summary>
    /// <param name="request">Данные в DTO для обновления поста.</param>
    /// <param name="existingCategory">Существующие категории.</param>
    /// <returns>Модель <see cref="Post"/>.</returns>
    public static Post ToModel(this PostUpdateRequest request, List<Category> existingCategory)
    {
        return new Post
        {
            Id = request.Id,
            Title = request.Title,
            Content = request.Content,
            PostCategories = existingCategory.Select(category =>
                new PostCategory
                {
                    PostId = request.Id,
                    CategoryId = category.Id,
                    Category = category
                }).ToList()
        };
    }

    /// <summary>
    /// <see cref="Post"/> в DTO <see cref="PostResponse"/>.
    /// </summary>
    /// <param name="post">Объект типа <see cref="Post"/>.</param>
    /// <returns><see cref="PostResponse"/>.</returns>
    public static PostResponse ToResponse(this Post? post)
    {
        return new PostResponse(post!.Id, post.Title, post.Content, post.UserId, post.PostCategories
            .Select(x =>
                new CategoryResponse(
                    x.CategoryId,
                    x.Category.Name))
            .ToList());
    }

    /// <summary>
    /// Коллекция <see cref="Post"/> в коллекцию DTO <see cref="PostResponse"/>.
    /// </summary>
    /// <param name="posts">Коллекция с объектами <see cref="Post"/>.</param>
    /// <returns>Коллекция с объектами <see cref="PostResponse"/>.</returns>
    public static List<PostResponse> ToResponse(this List<Post>? posts)
    {
        return posts!
            .Select(x => x.ToResponse())
            .ToList();
    }
}