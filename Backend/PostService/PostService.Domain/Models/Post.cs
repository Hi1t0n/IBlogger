using BaseLibrary.Classes;
using PostService.Domain.Contracts;

namespace PostService.Domain.Models;

/// <summary>
/// Пост.
/// </summary>
public class Post : BaseModel
{
    /// <summary>
    /// Заголовок.
    /// </summary>
    public string Title { get; set; } = string.Empty;
    
    /// <summary>
    /// Контент.
    /// </summary>
    public string Content { get; set; } = string.Empty;
    
    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public Guid UserId { get; set; } = Guid.Empty;
    
    /// <summary>
    /// Свойство связи.
    /// </summary>
    public ICollection<PostCategory> PostCategories { get; set; } = new List<PostCategory>();
}