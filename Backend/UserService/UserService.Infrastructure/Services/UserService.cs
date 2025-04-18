using System.Text.Json;
using BaseLibrary.Classes.Result;
using BaseLibrary.Constants;
using Microsoft.Extensions.Caching.Distributed;
using UserService.Domain.Contacts;
using UserService.Domain.Interfaces;
using UserService.Domain.Models;
using UserService.Infrastructure.Mappings;

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

    public async Task<Result<UserResponse?>> Add(AddUserRequest request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.Add(request.ToModel(), cancellationToken);

        if (user is null)
        {
            return Result<UserResponse>.Failed(string.Format(ResponseStringConstants.AddingErrorResponseStringTemplate, nameof(User)), 
                ResultType.BadRequest);
        }

        await _distributedCache.SetStringAsync(string.Format(RedisKeysConstants.EntityWithIdKeyTemplate, user.Id, nameof(User)),
            JsonSerializer.Serialize(user, OptionsConstants.JsonSerializerOptions),
            OptionsConstants.DistributedCacheEntryOptions,
            cancellationToken);

        return Result<UserResponse>.Success(user.ToResponse());
    }

    public async Task<Result<List<UserResponse>?>> Get(CancellationToken cancellationToken)
    {
        var cacheString =
            await _distributedCache.GetStringAsync(string.Format(RedisKeysConstants.AllEntityKeyTemplate, nameof(User)),
                cancellationToken);

        if (cacheString is not null)
        {
            var cacheUsers =
                JsonSerializer.Deserialize<List<User>?>(cacheString!, OptionsConstants.JsonSerializerOptions);

            return Result<List<UserResponse>?>.Success(cacheUsers!.ToResponse());
        }

        var users = await _userRepository.Get(cancellationToken);

        await _distributedCache.SetStringAsync(string.Format(RedisKeysConstants.AllEntityKeyTemplate, nameof(User)),
            JsonSerializer.Serialize(users, OptionsConstants.JsonSerializerOptions),
            OptionsConstants.DistributedCacheEntryOptions,
            cancellationToken);

        return Result<List<UserResponse>?>.Success(users!.ToResponse());
    }

    public async Task<Result<UserResponse?>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var cacheString =
            await _distributedCache.GetStringAsync(string.Format(RedisKeysConstants.EntityWithIdKeyTemplate, id, nameof(User)),
                cancellationToken);

        if (cacheString is not null)
        {
            var cacheUser = JsonSerializer.Deserialize<User>(cacheString!, OptionsConstants.JsonSerializerOptions);

            return Result<UserResponse?>.Success(cacheUser!.ToResponse());
        }

        var user = await _userRepository.GetById(id, cancellationToken);

        if (user is null)
        {
            return Result<UserResponse>.Failed(string.Format(ResponseStringConstants.NotFoundResponseStringTemplate, nameof(User),
                nameof(id).ToUpper(), id), ResultType.NotFound);
        }

        await _distributedCache.SetStringAsync(string.Format(RedisKeysConstants.EntityWithIdKeyTemplate, id, nameof(User)),
            JsonSerializer.Serialize(user, OptionsConstants.JsonSerializerOptions),
            OptionsConstants.DistributedCacheEntryOptions,
            cancellationToken);

        return Result<UserResponse?>.Success(user.ToResponse());
    }

    public async Task<Result<UserResponse?>> UpdateById(UpdateUserRequest request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.UpdateById(request.ToModel(), cancellationToken);

        if (user is null)
        {
            return Result<UserResponse>.Failed(string.Format(ResponseStringConstants.NotFoundResponseStringTemplate, nameof(User), 
                nameof(request.Id).ToUpper(), request.Id), ResultType.NotFound);
        }

        await _distributedCache.RemoveAsync(string.Format(RedisKeysConstants.EntityWithIdKeyTemplate, request.Id, nameof(User)),
            cancellationToken);

        return Result<UserResponse>.Success(user.ToResponse());
    }

    public async Task<Result<UserResponse?>> DeleteById(Guid id, CancellationToken cancellationToken)
    {
        var user = await _userRepository.DeleteById(id, cancellationToken);

        if (user is null)
        {
            return Result<UserResponse>.Failed(string.Format(ResponseStringConstants.NotFoundResponseStringTemplate,
                nameof(User), nameof(id).ToUpper(), id), ResultType.NotFound);
        }

        await _distributedCache.RemoveAsync(string.Format(RedisKeysConstants.EntityWithIdKeyTemplate, id, nameof(User)),
            cancellationToken);

        return Result<UserResponse>.Success(user.ToResponse());
    }

    public async Task<Result<UserResponse?>> RestoreUserByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var user = await _userRepository.RestoreUserByIdAsync(id, cancellationToken);

        if (user is null)
        {
            return Result<UserResponse>.Failed(string.Format(ResponseStringConstants.NotFoundResponseStringTemplate,
                nameof(User), nameof(id).ToUpper(), id), ResultType.NotFound);
        }

        return Result<UserResponse>.Success(user.ToResponse());
    }
}