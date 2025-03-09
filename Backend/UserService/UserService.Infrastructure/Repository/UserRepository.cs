using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using UserService.Domain;
using UserService.Domain.Interfaces;
using UserService.Domain.Models;
using UserService.Infrastructure.Context;

namespace UserService.Infrastructure.Repository;

///<inheritdoc cref="IUserRepository"/>
public class UserRepository(ApplicationDbContext context, IDistributedCache distributedCache) : IUserRepository
{
    private readonly ApplicationDbContext _context = context;
    private readonly IDistributedCache _distributedCache = distributedCache;
    
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
        var key = $"{id}_user";
        
        var user = new User();
        
        var cache = await _distributedCache.GetStringAsync(key, cancellationToken);

        if (!string.IsNullOrEmpty(cache))
        {
            user = JsonSerializer.Deserialize<User>(cache, Constants.JsonSerializerOptions);
            return user;
        }
        
        user = await _context.Users
            .Include(x=> x.Role)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.UserId == id && !x.IsDelete, cancellationToken);
            
        if (user is null)
        {
            return null;
        }

        await _distributedCache.SetStringAsync(key, JsonSerializer.Serialize(user, Constants.JsonSerializerOptions),
            Constants.DistributedCacheEntryOptions, cancellationToken);

        return user;
    }

    ///<inheritdoc/>
    public async Task<IReadOnlyCollection<User>?> GetUsersAsync(CancellationToken cancellationToken)
    {
        var key = $"users";

        var users = new List<User>();
        
        var cache = await _distributedCache.GetStringAsync(key, cancellationToken);
        
        
        if (!string.IsNullOrEmpty(cache))
        {
            users = JsonSerializer.Deserialize<List<User>>(cache, Constants.JsonSerializerOptions);
            return users;
        }
        
        users = await _context.Users
            .Include(x=> x.Role)
            .Where(x=> !x.IsDelete)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        await _distributedCache.SetStringAsync(key, JsonSerializer.Serialize(users, Constants.JsonSerializerOptions),
            Constants.DistributedCacheEntryOptions, cancellationToken);

        return users;
    }

    ///<inheritdoc/>
    public async Task<User?> UpdateUserByIdAsync(Guid id, User user, CancellationToken cancellationToken)
    {
        var updatingEntity = await _context.Users.FirstOrDefaultAsync(x => x.UserId == id, cancellationToken);

        if (updatingEntity is null)
        {
            return null;
        }

        updatingEntity.UserName = user.UserName;
        updatingEntity.Email = user.Email;
        updatingEntity.PhoneNumber = user.PhoneNumber;

        var result = _context.Users.Update(updatingEntity);
        await _context.SaveChangesAsync(cancellationToken);

        return result.Entity;
    }

    ///<inheritdoc/>
    public async Task<User?> DeleteUserByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var deletingUser =
            await _context.Users.FirstOrDefaultAsync(x => x.UserId == id && !x.IsDelete, cancellationToken);

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
            await _context.Users.FirstOrDefaultAsync(x => x.UserId == id && x.IsDelete == true, cancellationToken);

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