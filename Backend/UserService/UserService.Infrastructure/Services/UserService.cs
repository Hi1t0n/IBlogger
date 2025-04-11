using System.Text.Json;
using BaseLibrary.Classes.Result;
using Microsoft.Extensions.Caching.Distributed;
using UserService.Domain;
using UserService.Domain.Contacts;
using UserService.Domain.Interfaces;
using UserService.Domain.Models;
using UserService.Infrastructure.Extensions;

namespace UserService.Infrastructure.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IDistributedCache _distributedCache;
    
    public UserService(IUserRepository userRepository, IDistributedCache distributedCache)
    {
        _userRepository = userRepository;
        _distributedCache = distributedCache;
    }
    
    public async Task<Result<User?>> Add(AddUserRequest request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.Add(request.ToModel(), cancellationToken);

        if (user is null)
        {
            return Result<User>.Failed($"При добавлении {typeof(User)} что-то пошло не так", ResultType.BadRequest);
        }

        await _distributedCache.SetStringAsync($"{user.Id}_user",
            JsonSerializer.Serialize(user, OptionsConstants.JsonSerializerOptions), 
            OptionsConstants.DistributedCacheEntryOptions,
            cancellationToken);

        return Result<User>.Success(user);
    }
    
    public async Task<Result<IEnumerable<User>?>> Get(CancellationToken cancellationToken)
    {
        var cacheString = await _distributedCache.GetStringAsync("users", cancellationToken);

        if (cacheString is null)
        {
            var cacheUsers = JsonSerializer.Deserialize<IEnumerable<User>?>(cacheString!, OptionsConstants.JsonSerializerOptions);
            
            return Result<IEnumerable<User>?>.Success(cacheUsers);
        }

        var users = await _userRepository.Get(cancellationToken);

        await _distributedCache.SetStringAsync($"users",
            JsonSerializer.Serialize(users, OptionsConstants.JsonSerializerOptions),
            OptionsConstants.DistributedCacheEntryOptions,
            cancellationToken);

        return Result<IEnumerable<User>?>.Success(users);
    }

    public async Task<Result<User?>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var cacheString = await _distributedCache.GetStringAsync($"{id}_user", cancellationToken);

        if (cacheString is null)
        {
            var cacheUser = JsonSerializer.Deserialize<User>(cacheString!, OptionsConstants.JsonSerializerOptions);

            return Result<User?>.Success(cacheUser);
        }

        var user = await _userRepository.GetById(id, cancellationToken);

        if (user is null)
        {
            return Result<User>.Failed($"{nameof(User)} c Id: {id} не найден", ResultType.NotFound);
        }

        await _distributedCache.SetStringAsync($"{id}_user",
            JsonSerializer.Serialize(user, OptionsConstants.JsonSerializerOptions),
            OptionsConstants.DistributedCacheEntryOptions,
            cancellationToken);

        return Result<User?>.Success(user);
    }
    
    public async Task<Result<User?>> UpdateById(UpdateUserRequest request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.UpdateById(request.ToModel(), cancellationToken);

        if (user is null)
        {
            return Result<User>.Failed($"{nameof(User)} c Id: {request.Id} не найден", ResultType.NotFound);
        }

        await _distributedCache.RemoveAsync($"{request.Id}_user", cancellationToken);
        
        return Result<User>.Success(user);
    }

    public async Task<Result<User?>> DeleteById(Guid id, CancellationToken cancellationToken)
    {
        var user = await _userRepository.DeleteById(id, cancellationToken);

        if (user is null)
        {
            return Result<User>.Failed($"{nameof(User)} c Id: {id} не найден", ResultType.NotFound);
        }
        
        await _distributedCache.RemoveAsync($"{id}_user", cancellationToken); 
        
        return Result<User>.Success(user);
    }

    public async Task<Result<User?>> RestoreUserByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var user = await _userRepository.RestoreUserByIdAsync(id, cancellationToken);
        
        if (user is null)
        {
            return Result<User>.Failed($"{nameof(User)} c Id: {id} не найден", ResultType.NotFound);
        }
        
        return Result<User>.Success(user);
    }
}