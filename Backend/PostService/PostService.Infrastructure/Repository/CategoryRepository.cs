using BaseLibrary.Classes.Result;
using Microsoft.EntityFrameworkCore;
using PostService.Domain.Interfaces;
using PostService.Domain.Models;
using PostService.Infrastructure.Context;

namespace PostService.Infrastructure.Repository;

/// <summary>
/// <inheritdoc cref="ICategoryRepository"/>.
/// </summary>
/// <param name="context"></param>
public class CategoryRepository(ApplicationDbContext context) : ICategoryRepository
{
    /// <inheritdoc />.
    public async Task<Result<Category>> Add(Category entity, CancellationToken cancellationToken)
    {
        await context.Categories.AddAsync(entity, cancellationToken);

        return Result<Category>.Success(entity);
    }

    /// <inheritdoc />.
    public async Task<IEnumerable<Category>?> Get(CancellationToken cancellationToken)
    {
        return await context.Categories.ToListAsync(cancellationToken);
    }

    /// <inheritdoc />.
    public async Task<Result<Category>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (category is null)
        {
            return Result<Category>.Failed($"{nameof(Category)} с Id: {id} не найден.", ResultType.NotFound)!;
        }

        return Result<Category>.Success(category);
    }

    /// <inheritdoc />.
    public async Task<List<Category>> GetExistCategories(List<Guid> categories)
    {
        return await context.Categories
            .Where(x => categories.Contains(x.Id))
            .ToListAsync();
    }

    /// <inheritdoc />.
    public async Task<Result<Category>> UpdateById(Category updateData, CancellationToken cancellationToken)
    {
        var category = await context.Categories
            .FirstOrDefaultAsync(x => x.Id == updateData.Id, cancellationToken);

        if (category is null)
        {
            return Result<Category>.Failed($"{nameof(Category)} c Id: {updateData.Id} не найден", ResultType.NotFound)!;
        }

        category.Name = updateData.Name;

        await context.SaveChangesAsync(cancellationToken);

        return Result<Category>.Success(category);
    }

    /// <inheritdoc />.
    public async Task<Result<Category>> DeleteById(Guid id, CancellationToken cancellationToken)
    {
        var category = await context.Categories
            .Include(x => x.PostCategories)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (category is null)
        {
            return Result<Category>.Failed($"{nameof(Category)} с Id: {id} не найден", ResultType.NotFound)!;
        }

        var strategy = context.Database.CreateExecutionStrategy();

        await strategy.ExecuteAsync(async () =>
        {
            try
            {
                await context.Database.BeginTransactionAsync(cancellationToken);

                if (category.PostCategories.Any())
                {
                    context.PostCategories.RemoveRange(category.PostCategories);
                }

                context.Categories.Remove(category);

                await context.Database.CommitTransactionAsync(cancellationToken);
            }
            catch (Exception exception)
            {
                await context.Database.RollbackTransactionAsync(cancellationToken);
                throw;
            }
        });

        return Result<Category>.Success(category);
    }
}