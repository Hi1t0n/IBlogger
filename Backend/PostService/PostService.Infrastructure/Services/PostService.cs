using System.Text.Json;
using BaseLibrary.Classes.Result;
using BaseLibrary.Constants;
using Microsoft.Extensions.Caching.Distributed;
using PostService.Domain.Contracts.PostContracts;
using PostService.Domain.Interfaces;
using PostService.Domain.Models;
using PostService.Infrastructure.Mappings;

namespace PostService.Infrastructure.Services;

/// <inheritdoc cref="IPostService"/>
public class PostService : IPostService
{
    /// <summary>
    /// <inheritdoc cref="IPostRepository"/>
    /// </summary>
    private readonly IPostRepository _postRepository;

    /// <summary>
    /// <inheritdoc cref="ICategoryRepository"/>
    /// </summary>
    private readonly ICategoryRepository _categoryRepository;

    private readonly IDistributedCache _distributedCache;

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="postRepository"><see cref="IPostRepository"/>.</param>
    /// <param name="categoryRepository"><see cref="ICategoryRepository"/>.</param>
    /// <param name="distributedCache"><see cref="IDistributedCache"/></param>
    public PostService(IPostRepository postRepository, ICategoryRepository categoryRepository, IDistributedCache distributedCache)
    {
        _postRepository = postRepository;
        _categoryRepository = categoryRepository;
        _distributedCache = distributedCache;
    }

    /// <inheritdoc/>
    public async Task<Result<Post?>> AddPost(AddPostRequest request, CancellationToken cancellationToken)
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

    /// <inheritdoc/>
    public async Task<Result<IEnumerable<Post>?>> Get(CancellationToken cancellationToken)
    {
        var cacheString = await _distributedCache.GetStringAsync(
            string.Format(RedisKeysConstants.AllEntityKeyTemplate, nameof(Post)),
            cancellationToken);

        if (cacheString is not null)
        {
            var cachePosts =
                JsonSerializer.Deserialize<IEnumerable<Post>>(cacheString, OptionsConstants.JsonSerializerOptions);
            return Result<IEnumerable<Post>>.Success(cachePosts);
        }

        var posts = await _postRepository.Get(cancellationToken);

        if (posts is not null)
        {
            await _distributedCache.SetStringAsync(string.Format(RedisKeysConstants.AllEntityKeyTemplate, nameof(Post)),
                JsonSerializer.Serialize(posts, OptionsConstants.JsonSerializerOptions),
                OptionsConstants.DistributedCacheEntryOptions,
                cancellationToken);
        }

        return Result<IEnumerable<Post>>.Success(posts);
    }

    /// <inheritdoc/>
    public async Task<Result<Post?>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var cacheString = await _distributedCache.GetStringAsync(
            string.Format(RedisKeysConstants.EntityWithIdKeyTemplate, id, nameof(Post)),
            cancellationToken);

        if (cacheString is not null)
        {
            var cachePost = JsonSerializer.Deserialize<Post>(cacheString, OptionsConstants.JsonSerializerOptions);

            Result<Post>.Success(cachePost);
        }

        var post = await _postRepository.GetById(id, cancellationToken);

        if (post is not null)
        {
            await _distributedCache.SetStringAsync(string.Format(RedisKeysConstants.EntityWithIdKeyTemplate, id, nameof(Post)),
                JsonSerializer.Serialize(post, OptionsConstants.JsonSerializerOptions),
                OptionsConstants.DistributedCacheEntryOptions,
                cancellationToken);
        }

        return post is not null
            ? Result<Post>.Failed(string.Format(ResponseStringConstants.NotFoundResponseStringTemplate, nameof(Post),
                nameof(id).ToUpper(), id), ResultType.NotFound)
            : Result<Post>.Success(post);
    }

    /// <inheritdoc/>
    public async Task<Result<IEnumerable<Post>?>> GetPostsByUserId(Guid userId, CancellationToken cancellationToken)
    {
        var cacheString = await _distributedCache.GetStringAsync(
            string.Format(RedisKeysConstants.EntityWithIdKeyTemplate, userId, $"{nameof(Post)}User"),
            cancellationToken);

        if (cacheString is not null)
        {
            var cacheUserPosts =
                JsonSerializer.Deserialize<IEnumerable<Post>>(cacheString, OptionsConstants.JsonSerializerOptions);

            return Result<IEnumerable<Post>>.Success(cacheUserPosts);
        }

        var posts = await _postRepository.GetPostsByUserId(userId, cancellationToken);

        if (posts is not null)
        {
            await _distributedCache.SetStringAsync(string.Format(RedisKeysConstants.EntityWithIdKeyTemplate, userId, $"{nameof(Post)}User"),
                JsonSerializer.Serialize(posts, OptionsConstants.JsonSerializerOptions), 
                OptionsConstants.DistributedCacheEntryOptions, 
                cancellationToken);
        }

        return Result<IEnumerable<Post>>.Success(posts);
    }

    /// <inheritdoc/>
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
            return Result<Post>.Failed(string.Format(ResponseStringConstants.NotFoundResponseStringTemplate,
                nameof(Post),
                nameof(request.Id).ToUpper(), request.Id), ResultType.NotFound);
        }

        await _distributedCache.RemoveAsync(string.Format(RedisKeysConstants.EntityWithIdKeyTemplate, request.Id, nameof(Post)),
            cancellationToken);

        return Result<Post>.Success(updatedPost);
    }

    /// <inheritdoc/>
    public async Task<Result<Post?>> DeleteById(Guid id, CancellationToken cancellationToken)
    {
        var post = await _postRepository.DeleteById(id, cancellationToken);

        if (post is null)
        {
            return Result<Post>.Failed(string.Format(ResponseStringConstants.NotFoundResponseStringTemplate,
                nameof(Post),
                nameof(id).ToUpper(), id), ResultType.NotFound);
        }
        
        await _distributedCache.RemoveAsync(string.Format(RedisKeysConstants.EntityWithIdKeyTemplate, id, nameof(Post)),
            cancellationToken);

        return Result<Post>.Success(post);
    }
}