using BaseLibrary.Classes.Result;
using Microsoft.EntityFrameworkCore;
using PostService.Domain.Interfaces;
using PostService.Domain.Models;
using PostService.Infrastructure.Context;

namespace PostService.Infrastructure.Repository;

/// <inheritdoc cref="IPostRepository"/>. 
public class PostRepository(ApplicationDbContext context) : IPostRepository
{
    /// <inheritdoc/>.
    public async Task<Post?> Add(Post entity, CancellationToken cancellationToken)
    {
        if (!entity.PostCategories.Any())
        {
            Result<Post>.Failed($"{nameof(entity.PostCategories)} не может быть пустым.", ResultType.BadRequest);
        }

        try
        {
            var strategy = context.Database.CreateExecutionStrategy();
            
            await strategy.ExecuteAsync(async () =>
            {
                await context.Database.BeginTransactionAsync(cancellationToken);
                await context.Posts.AddAsync(entity, cancellationToken);

                await context.SaveChangesAsync(cancellationToken);
                await context.Database.CommitTransactionAsync(cancellationToken);
            });
            
            return entity;
        }
        catch (Exception exception)
        {
            await context.Database.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

    /// <inheritdoc/>.
    public async Task<IEnumerable<Post>?> Get(CancellationToken cancellationToken)
    {
        return await context.Posts
            .Include(x => x.PostCategories)
            .ThenInclude(x => x.Category)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>.
    public async Task<Post?> GetById(Guid id, CancellationToken cancellationToken)
    {
        var post = await context.Posts
            .Include(x => x.PostCategories)
            .ThenInclude(x => x.Category)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return post;
    }

    /// <inheritdoc/>.
    public async Task<List<Post>?> GetPostsByUserId(Guid userId, CancellationToken cancellationToken)
    {
        return await context.Posts
            .Include(x => x.PostCategories)
            .ThenInclude(x => x.Category)
            .Where(x => x.UserId == userId)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>.
    public async Task<Post?> UpdateById(Post updateData, CancellationToken cancellationToken)
    {
        var post = await context.Posts
            .Include(x => x.PostCategories)
            .FirstOrDefaultAsync(x => x.Id == updateData.Id, cancellationToken);

        if (post is null)
        {
            return null;
        }

        try
        {
            await context.Database.BeginTransactionAsync(cancellationToken);

            post.Content = updateData.Content;
            post.Title = updateData.Title;
            post.ModifiedOn = DateTime.UtcNow;

            if (updateData.PostCategories.Any())
            {
                context.PostCategories.RemoveRange(post.PostCategories);
            }

            await context.PostCategories.AddRangeAsync(updateData.PostCategories, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);
            await context.Database.CommitTransactionAsync(cancellationToken);

            return post;
        }
        catch (Exception exception)
        {
            await context.Database.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

    /// <inheritdoc/>.
    public async Task<Post?> DeleteById(Guid id, CancellationToken cancellationToken)
    {
        var post = await context.Posts
            .Include(x => x.PostCategories)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (post is null)
        {
            return null;
        }

        try
        {
            var strategy = context.Database.CreateExecutionStrategy();
            
            await strategy.ExecuteAsync(async () =>
            {
                await context.Database.BeginTransactionAsync(cancellationToken);

                context.PostCategories.RemoveRange(post.PostCategories);
                context.Posts.Remove(post);

                await context.SaveChangesAsync(cancellationToken);
                await context.Database.CommitTransactionAsync(cancellationToken);
            });

            return post;
        }
        catch (Exception exception)
        {
            await context.Database.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}