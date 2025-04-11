using BaseLibrary.Classes.Result;
using BaseLibrary.Interfaces;
using PostService.Domain.Contracts.PostContracts;
using PostService.Domain.Models;

namespace PostService.Domain.Interfaces;

public interface IPostService : IService<Post>
{
    public Task<Result<Post?>> Add(AddPostRequest request, CancellationToken cancellationToken);
    public Task<Result<List<Post>?>> GetPostsByUserId(Guid userId, CancellationToken cancellationToken);
    public Task<Result<Post?>> UpdateById(PostUpdateRequest request, CancellationToken cancellationToken);
}