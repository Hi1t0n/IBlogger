using Microsoft.EntityFrameworkCore;
using UserService.Domain.Interfaces;
using UserService.Domain.Models;
using UserService.Infrastructure.Context;

namespace UserService.Infrastructure.Repository;

///<inheritdoc cref="IUserRepository"/>
public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    ///<inheritdoc/>
    public async Task<User?> AddUserAsync(User user, CancellationToken cancellationToken)
    {
        var result = await _context.Users.AddAsync(user, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return result.Entity;
    }

    ///<inheritdoc/>
    public async Task<User?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .Include(x => x.Role)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDelete, cancellationToken);

        return user is not null
            ? user
            : null;
    }

    ///<inheritdoc/>
    public async Task<List<User>?> GetUsersAsync(CancellationToken cancellationToken)
    {
        var users = await _context.Users
            .Include(x => x.Role)
            .Where(x => !x.IsDelete)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return users;
    }

    ///<inheritdoc/>
    public async Task<int?> UpdateUserById(User user, CancellationToken cancellationToken)
    {
        var updatingResult = await _context.Users
            .Where(x => x.Id == user.Id)
            .ExecuteUpdateAsync(setter => setter
                .SetProperty(x => x.UserName, user.UserName)
                .SetProperty(x => x.Email, user.Email)
                .SetProperty(x => x.PhoneNumber, user.PhoneNumber)
                .SetProperty(x => x.ModifiedOn, DateTime.UtcNow), cancellationToken);


        if (updatingResult <= 0)
        {
            return null;
        }

        return updatingResult;
    }

    ///<inheritdoc/>
    public async Task<int?> DeleteUserById(Guid id, CancellationToken cancellationToken)
    {
        var deletingResult = await _context.Users
            .Where(x => x.Id == id && !x.IsDelete)
            .ExecuteUpdateAsync(setter => setter
                .SetProperty(x => x.IsDelete, true), cancellationToken);

        if (deletingResult <= 0)
        {
            return null;
        }


        return deletingResult;
    }

    ///<inheritdoc/>
    public async Task<int?> RestoreUserByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var restoringResult = await _context.Users
            .Where(x => x.Id == id && x.IsDelete)
            .ExecuteUpdateAsync(setter => setter
                .SetProperty(x => x.IsDelete, false), cancellationToken);

        if (restoringResult <= 0)
        {
            return null;
        }

        return restoringResult;
    }

    ///<inheritdoc/>
    public async Task<bool> ExistByEmailAsync(string? email)
    {
        return await _context.Users.AnyAsync(x => x.Email == email);
    }

    ///<inheritdoc/>
    public async Task<bool> ExistByPhoneNumberAsync(string? phoneNumber)
    {
        return await _context.Users.AnyAsync(x => x.PhoneNumber == phoneNumber);
    }

    ///<inheritdoc/>
    public async Task<bool> ExistByUserNameAsync(string userName)
    {
        return await _context.Users.AnyAsync(x => x.UserName == userName);
    }
}