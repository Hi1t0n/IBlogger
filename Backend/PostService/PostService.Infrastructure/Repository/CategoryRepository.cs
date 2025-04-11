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
    public async Task<Category?> Add(Category entity, CancellationToken cancellationToken)
    {
        await context.Categories.AddAsync(entity, cancellationToken);

        return entity;
    }

    /// <inheritdoc />.
    public async Task<IEnumerable<Category>?> Get(CancellationToken cancellationToken)
    {
        return await context.Categories.ToListAsync(cancellationToken);
    }

    /// <inheritdoc />.
    public async Task<Category?> GetById(Guid id, CancellationToken cancellationToken)
    {
        var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (category is null)
        {
            return null;
        }

        return category;
    }

    /// <inheritdoc />.
    public async Task<List<Category>?> GetExistCategories(List<Guid> categories)
    {
        return await context.Categories
            .Where(x => categories.Contains(x.Id))
            .ToListAsync();
    }

    /// <inheritdoc />.
    public async Task<Category?> UpdateById(Category updateData, CancellationToken cancellationToken)
    {
        var category = await context.Categories
            .FirstOrDefaultAsync(x => x.Id == updateData.Id, cancellationToken);

        if (category is null)
        {
            return null;
        }

        category.Name = updateData.Name;

        await context.SaveChangesAsync(cancellationToken);

        return category;
    }

    /// <inheritdoc />.
    public async Task<Category?> DeleteById(Guid id, CancellationToken cancellationToken)
    {
        var category = await context.Categories
            .Include(x => x.PostCategories)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (category is null)
        {
            return null;
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

        return category;
    }
}