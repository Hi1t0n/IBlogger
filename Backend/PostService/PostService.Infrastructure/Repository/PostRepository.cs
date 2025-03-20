using BaseLibrary.Classes;
using BaseLibrary.Classes.Result;
using Microsoft.EntityFrameworkCore;
using PostService.Domain.Interfaces;
using PostService.Domain.Models;
using PostService.Infrastructure.Context;

namespace PostService.Infrastructure.Repository;

public class PostRepository(ApplicationDbContext context) : IPostRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result<Post>> Create(Post entity, CancellationToken cancellationToken)
    {
        try
        {
            await _context.Database.BeginTransactionAsync(cancellationToken);

            await _context.Posts.AddAsync(entity, cancellationToken);

            if (!entity.PostCategories.Any())
            {
                Result<Post>.Failed($"{nameof(entity.PostCategories)} не может быть пустым.", ResultType.BadRequest);
            }

            await _context.AddRangeAsync(entity.PostCategories, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);
            await _context.Database.CommitTransactionAsync(cancellationToken);

            return Result<Post>.Success(entity);
        }
        catch (Exception exception)
        {
            await _context.Database.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

    public async Task<IEnumerable<Post>?> Get(CancellationToken cancellationToken)
    {
        return await _context.Posts
            .Include(x => x.PostCategories)
            .ThenInclude(x => x.Category)
            .ToListAsync(cancellationToken);
    }

    public async Task<Result<Post>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var post = await _context.Posts
            .Include(x => x.PostCategories)
            .ThenInclude(x => x.Category)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (post is null)
        {
            return Result<Post>.Failed($"{nameof(Post)} с Id: {id} не найден.", ResultType.NotFound)!;
        }

        return Result<Post>.Success(post);
    }

    public async Task<Result<Post>> UpdateById(Guid id, Post updateData, CancellationToken cancellationToken)
    {
        var post = await _context.Posts
            .Include(x=> x.PostCategories)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (post is null)
        {
            return Result<Post>.Failed($"{nameof(Post)} c Id: {id} не найден", ResultType.NotFound)!;
        }

        try
        {
            await _context.Database.BeginTransactionAsync(cancellationToken);

            post.Content = updateData.Content;
            post.Name = updateData.Name;
            post.ModifiedOn = DateTime.Now;

            if (updateData.PostCategories.Any())
            {
                _context.PostCategories.RemoveRange(post.PostCategories);
            }
            
            await _context.PostCategories.AddRangeAsync(updateData.PostCategories, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);
            await _context.Database.CommitTransactionAsync(cancellationToken);

            return Result<Post>.Success(post);
        }
        catch (Exception exception)
        {
            await _context.Database.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

    public async Task<Result<Post>> DeleteById(Guid id, CancellationToken cancellationToken)
    {
        var post = await _context.Posts
            .Include(x=> x.PostCategories)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (post is null)
        {
            return Result<Post>.Failed($"{nameof(Post)} с Id: {id} не найден", ResultType.NotFound)!;
        }

        try
        {
            await _context.Database.BeginTransactionAsync(cancellationToken);
            
            _context.PostCategories.RemoveRange(post.PostCategories);
            _context.Posts.Remove(post);

            await _context.SaveChangesAsync(cancellationToken);
            await _context.Database.CommitTransactionAsync(cancellationToken);

            return Result<Post>.Success(post);
        }
        catch (Exception exception)
        {
            await _context.Database.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}