namespace PostService.Domain.Models;

/// <summary>
/// Посты категории (Модель связи многие ко многим).
/// </summary>
public class PostCategory
{
    /// <summary>
    /// Идентификатор поста.
    /// </summary>
    public Guid PostId = Guid.Empty;
    
    /// <summary>
    /// Идентификатор категории.
    /// </summary>
    public Guid CategoryId = Guid.Empty;
    
    /// <summary>
    /// Свойство для связи.
    /// </summary>
    public Post Post { get; set; } = new Post();
    
    /// <summary>
    /// Свойство для связи.
    /// </summary>
    public Category Category { get; set; } = new Category();
}