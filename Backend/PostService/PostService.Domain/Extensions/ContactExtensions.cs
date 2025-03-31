using Microsoft.AspNetCore.Builder;
using PostService.Domain.Contracts;
using PostService.Domain.Models;

namespace PostService.Domain.Extensions;

/// <summary>
/// Методы расширения для контрактов.
/// </summary>
public static class ContactExtensions
{
    /// <summary>
    /// Конвертирует <see cref="AddPostContract"/> в <see cref="Post"/>.
    /// </summary>
    /// <param name="contract">Объект с данными.</param>
    /// <returns>Объект <see cref="Post"/>.</returns>
    public static Post ToModel(this AddPostContract contract)
    {
        var postId = Guid.NewGuid();

        var post = new Post()
        {
            Id = postId,
            Title = contract.Title,
            UserId = contract.UserId,
            PostCategories = contract.Categories.Select(categoryId => new PostCategory()
            {
                PostId = postId,
                CategoryId = categoryId
            }).ToList(),
            CreatedOn = DateTime.Now,
            ModifiedOn = DateTime.Now
        };

        return post;
    }
}