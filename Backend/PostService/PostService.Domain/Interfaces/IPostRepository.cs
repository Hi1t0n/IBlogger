using BaseLibrary.Interfaces;
using PostService.Domain.Models;

namespace PostService.Domain.Interfaces;

/// <summary>
/// Репозиторий <see cref="Post"/>.
/// </summary>
public interface IPostRepository : IRepository<Post>
{
    
}