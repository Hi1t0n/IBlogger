using BaseLibrary.Classes;

namespace PostService.Domain.Models;

/// <summary>
/// Категория.
/// </summary>
public class Category : BaseModel
{
    /// <summary>
    /// Название.
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Свойство для связи.
    /// </summary>
    public ICollection<PostCategory> PostCategories { get; set; } = new List<PostCategory>();
}