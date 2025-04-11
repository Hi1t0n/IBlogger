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
    public async Task<User?> Add(User user, CancellationToken cancellationToken)
    {
        var result = await _context.Users.AddAsync(user, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return result.Entity;
    }

    ///<inheritdoc/>
    public async Task<User?> GetById(Guid id, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .Include(x=> x.Role)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDelete, cancellationToken);

        return user is not null
            ? user
            : null;
    }

    ///<inheritdoc/>
    public async Task<IEnumerable<User>?> Get(CancellationToken cancellationToken)
    {
        var users = await _context.Users
            .Include(x=> x.Role)
            .Where(x=> !x.IsDelete)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return users;
    }

    ///<inheritdoc/>
    public async Task<User?> UpdateById(User user, CancellationToken cancellationToken)
    {
        var updatingEntity = await _context.Users.FirstOrDefaultAsync(x => x.Id == user.Id, cancellationToken);

        if (updatingEntity is null)
        {
            return null;
        }

        updatingEntity.UserName = user.UserName;
        updatingEntity.Email = user.Email;
        updatingEntity.PhoneNumber = user.PhoneNumber;
        updatingEntity.ModifiedOn = DateTime.UtcNow;

        var result = _context.Users.Update(updatingEntity);
        await _context.SaveChangesAsync(cancellationToken);

        return result.Entity;
    }

    ///<inheritdoc/>
    public async Task<User?> DeleteById(Guid id, CancellationToken cancellationToken)
    {
        var deletingUser =
            await _context.Users.FirstOrDefaultAsync(x => x.Id == id && !x.IsDelete, cancellationToken);

        if (deletingUser is null)
        {
            return null;
        }

        deletingUser.IsDelete = true;

        var result = _context.Users.Update(deletingUser);
        await _context.SaveChangesAsync(cancellationToken);
        

        return result.Entity;
    }

    ///<inheritdoc/>
    public async Task<User?> RestoreUserByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var restoringUser =
            await _context.Users.FirstOrDefaultAsync(x => x.Id == id && x.IsDelete == true, cancellationToken);

        if (restoringUser is null)
        {
            return null;
        }

        restoringUser.IsDelete = false;

        var result = _context.Users.Update(restoringUser);

        await _context.SaveChangesAsync(cancellationToken);
        
        return result.Entity;
    }

    ///<inheritdoc/>
    public async Task<bool> ExistByEmail(string? email)
    {
        return await _context.Users.AnyAsync(x => x.Email == email);
    }

    ///<inheritdoc/>
    public async Task<bool> ExistByPhoneNumber(string? phoneNumber)
    {
        return await _context.Users.AnyAsync(x => x.PhoneNumber == phoneNumber);
    }

    ///<inheritdoc/>
    public async Task<bool> ExistByUserName(string userName)
    {
        return await _context.Users.AnyAsync(x => x.UserName == userName);
    }
}