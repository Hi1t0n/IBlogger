using BaseLibrary.Classes.Result;
using BaseLibrary.Constants;
using PostService.Domain.Contracts.PostContracts;
using PostService.Domain.Interfaces;
using PostService.Domain.Models;
using PostService.Infrastructure.Mappings;

namespace PostService.Infrastructure.Services;

public class PostService : IPostService
{
    private readonly IPostRepository _postRepository;
    private readonly ICategoryRepository _categoryRepository;

    public PostService(IPostRepository postRepository, ICategoryRepository categoryRepository)
    {
        _postRepository = postRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<Result<Post?>> Add(AddPostRequest request, CancellationToken cancellationToken)
    {
        var existCategories = await _categoryRepository.GetExistCategories(request.Categories);

        if (!existCategories.Any())
        {
            return Result<Post>.Failed(string.Format(ResponseStringConstants.RequiredAttributeResponseStringTemplate,
                    nameof(Post.PostCategories)), ResultType.BadRequest);
        }

        var post = await _postRepository.Add(request.ToModel(existCategories), cancellationToken);

        return Result<Post>.Success(post);
    }

    public async Task<Result<IEnumerable<Post>?>> Get(CancellationToken cancellationToken)
    {
        var posts = await _postRepository.Get(cancellationToken);

        return Result<IEnumerable<Post>>.Success(posts);
        ;
    }

    public async Task<Result<Post?>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var post = await _postRepository.GetById(id, cancellationToken);

        return post is not null
            ? Result<Post>.Failed(string.Format(ResponseStringConstants.NotFoundResponseStringTemplate, nameof(Post),
                    nameof(id).ToUpper(), id), ResultType.NotFound)
            : Result<Post>.Success(post);
    }

    public async Task<Result<List<Post>?>> GetPostsByUserId(Guid userId, CancellationToken cancellationToken)
    {
        var posts = await _postRepository.GetPostsByUserId(userId, cancellationToken);

        return Result<List<Post>>.Success(posts);
    }

    public async Task<Result<Post?>> UpdateById(PostUpdateRequest request, CancellationToken cancellationToken)
    {
        var existingCategory = await _categoryRepository.GetExistCategories(request.Categories);

        if (!existingCategory.Any())
        {
            return Result<Post>.Failed(string.Format(ResponseStringConstants.RequiredAttributeResponseStringTemplate,
                nameof(Post.PostCategories)), ResultType.BadRequest);
        }

        var updatedPost = await _postRepository.UpdateById(request.ToModel(existingCategory), cancellationToken);

        if (updatedPost is null)
        {
            return Result<Post>.Failed(string.Format(ResponseStringConstants.NotFoundResponseStringTemplate, nameof(Post),
                nameof(request.Id).ToUpper(), request.Id), ResultType.NotFound);
        }

        return Result<Post>.Success(updatedPost);
    }

    public async Task<Result<Post?>> DeleteById(Guid id, CancellationToken cancellationToken)
    {
        var post = await _postRepository.DeleteById(id, cancellationToken);

        if (post is null)
        {
            return Result<Post>.Failed(string.Format(ResponseStringConstants.NotFoundResponseStringTemplate, nameof(Post),
                nameof(id).ToUpper(), id), ResultType.NotFound);
        }

        return Result<Post>.Success(post);
    }
}