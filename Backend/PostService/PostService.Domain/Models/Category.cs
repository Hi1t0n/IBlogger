using BaseLibrary.Classes;

namespace PostService.Domain.Models;

public class Category : BaseModel
{
    public string Name { get; set; } = string.Empty;
    public ICollection<PostCategory> PostCategories { get; set; } = new List<PostCategory>();
}