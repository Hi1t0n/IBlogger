namespace PostService.Domain.Models;

public class Post
{
    public Guid Id { get; set; } = Guid.Empty;
    public string Name { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public Guid UserId { get; set; } = Guid.Empty;
    public DateTime CreatedOn { get; set; }
    public DateTime ModifiedAt { get; set; }
    public ICollection<PostCategory> PostCategories { get; set; } = new List<PostCategory>();
}