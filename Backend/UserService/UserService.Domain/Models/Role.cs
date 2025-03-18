using BaseLibrary.Classes;

namespace UserService.Domain.Models;

/// <summary>
/// Роль.
/// </summary>
public class Role : BaseModel
{
    /// <summary>
    /// Название роли.
    /// </summary>
    public string? RoleName { get; set; } = string.Empty;
    
    /// <summary>
    /// Связывающее свойство с <see cref="Users"/>.
    /// </summary>
    public ICollection<User> Users { get; set; } = new List<User>();
}