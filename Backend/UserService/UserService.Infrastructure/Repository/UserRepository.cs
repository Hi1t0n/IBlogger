using System.Text.Json;
using BaseLibrary.Classes.Result;
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
    public async Task<Result<User>> Create(User user, CancellationToken cancellationToken)
    {
        var result = await _context.Users.AddAsync(user, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return Result<User>.Success(result.Entity);
    }

    ///<inheritdoc/>
    public async Task<Result<User>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var key = $"{id}_user";
        
        var cache = await _distributedCache.GetStringAsync(key, cancellationToken);

        if (!string.IsNullOrEmpty(cache))
        {
            var cacheUser = JsonSerializer.Deserialize<User>(cache, Constants.JsonSerializerOptions);
            return Result<User>.Success(cacheUser!);
        }
        
        var user = await _context.Users
            .Include(x=> x.Role)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.UserId == id && !x.IsDelete, cancellationToken);
            
        if (user is null)
        {
            return Result<User>.Failed($"{nameof(User)} c Id: {id} не найден", ResultType.NotFound)!;
        }

        await _distributedCache.SetStringAsync(key, JsonSerializer.Serialize(user, Constants.JsonSerializerOptions),
            Constants.DistributedCacheEntryOptions, cancellationToken);

        return Result<User>.Success(user);
    }

    ///<inheritdoc/>
    public async Task<IEnumerable<User>?> Get(CancellationToken cancellationToken)
    {
        var key = $"users";
        
        var cache = await _distributedCache.GetStringAsync(key, cancellationToken);
        
        if (!string.IsNullOrEmpty(cache))
        {
            var cachedUsers = JsonSerializer.Deserialize<List<User>>(cache, Constants.JsonSerializerOptions);
            return cachedUsers;
        }
        
        var users = await _context.Users
            .Include(x=> x.Role)
            .Where(x=> !x.IsDelete)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        await _distributedCache.SetStringAsync(key, JsonSerializer.Serialize(users, Constants.JsonSerializerOptions),
            Constants.DistributedCacheEntryOptions, cancellationToken);

        return users;
    }

    ///<inheritdoc/>
    public async Task<Result<User>> UpdateById(Guid id, User user, CancellationToken cancellationToken)
    {
        var updatingEntity = await _context.Users.FirstOrDefaultAsync(x => x.UserId == id, cancellationToken);

        if (updatingEntity is null)
        {
            return Result<User>.Failed($"{nameof(User)} c Id: {id} не найден", ResultType.NotFound)!;
        }

        updatingEntity.UserName = user.UserName;
        updatingEntity.Email = user.Email;
        updatingEntity.PhoneNumber = user.PhoneNumber;

        var result = _context.Users.Update(updatingEntity);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<User>.Success(result.Entity);
    }

    ///<inheritdoc/>
    public async Task<Result<User>> DeleteById(Guid id, CancellationToken cancellationToken)
    {
        var deletingUser =
            await _context.Users.FirstOrDefaultAsync(x => x.UserId == id && !x.IsDelete, cancellationToken);

        if (deletingUser is null)
        {
            return Result<User>.Failed($"{nameof(User)} c Id: {id} не найден", ResultType.NotFound)!;
        }

        deletingUser.IsDelete = true;

        var result = _context.Users.Update(deletingUser);
        await _context.SaveChangesAsync(cancellationToken);
        

        return Result<User>.Success(result.Entity);
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