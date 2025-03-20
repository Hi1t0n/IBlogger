using BaseLibrary.Classes;

namespace PostService.Domain.Models;

public class Post : BaseModel
{
    public string Name { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public Guid UserId { get; set; } = Guid.Empty;
    public ICollection<PostCategory> PostCategories { get; set; } = new List<PostCategory>();
}