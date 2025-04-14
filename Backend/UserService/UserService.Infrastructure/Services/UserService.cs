using System.Text.Json;
using BaseLibrary.Classes.Result;
using BaseLibrary.Constants;
using Microsoft.Extensions.Caching.Distributed;
using UserService.Domain;
using UserService.Domain.Constants;
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
            return Result<User>.Failed(string.Format(ResponseStringConstants.AddingErrorResponseStringTemplate, nameof(User)), 
                ResultType.BadRequest);
        }

        await _distributedCache.SetStringAsync(string.Format(RedisKeysConstants.EntityWithIdKeyTemplate, user.Id, nameof(User)),
            JsonSerializer.Serialize(user, OptionsConstants.JsonSerializerOptions),
            OptionsConstants.DistributedCacheEntryOptions,
            cancellationToken);

        return Result<User>.Success(user);
    }

    public async Task<Result<IEnumerable<User>?>> Get(CancellationToken cancellationToken)
    {
        var cacheString =
            await _distributedCache.GetStringAsync(string.Format(RedisKeysConstants.AllEntityKeyTemplate, nameof(User)),
                cancellationToken);

        if (cacheString is null)
        {
            var cacheUsers =
                JsonSerializer.Deserialize<IEnumerable<User>?>(cacheString!, OptionsConstants.JsonSerializerOptions);

            return Result<IEnumerable<User>?>.Success(cacheUsers);
        }

        var users = await _userRepository.Get(cancellationToken);

        await _distributedCache.SetStringAsync(string.Format(RedisKeysConstants.AllEntityKeyTemplate, nameof(User)),
            JsonSerializer.Serialize(users, OptionsConstants.JsonSerializerOptions),
            OptionsConstants.DistributedCacheEntryOptions,
            cancellationToken);

        return Result<IEnumerable<User>?>.Success(users);
    }

    public async Task<Result<User?>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var cacheString =
            await _distributedCache.GetStringAsync(string.Format(RedisKeysConstants.EntityWithIdKeyTemplate, id, nameof(User)),
                cancellationToken);

        if (cacheString is null)
        {
            var cacheUser = JsonSerializer.Deserialize<User>(cacheString!, OptionsConstants.JsonSerializerOptions);

            return Result<User?>.Success(cacheUser);
        }

        var user = await _userRepository.GetById(id, cancellationToken);

        if (user is null)
        {
            return Result<User>.Failed(string.Format(ResponseStringConstants.NotFoundResponseStringTemplate, nameof(User),
                nameof(id).ToUpper(), id), ResultType.NotFound);
        }

        await _distributedCache.SetStringAsync(string.Format(RedisKeysConstants.EntityWithIdKeyTemplate, id, nameof(User)),
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
            return Result<User>.Failed(string.Format(ResponseStringConstants.NotFoundResponseStringTemplate, nameof(User), 
                nameof(request.Id).ToUpper(), request.Id), ResultType.NotFound);
        }

        await _distributedCache.RemoveAsync(string.Format(RedisKeysConstants.EntityWithIdKeyTemplate, request.Id, nameof(User)),
            cancellationToken);

        return Result<User>.Success(user);
    }

    public async Task<Result<User?>> DeleteById(Guid id, CancellationToken cancellationToken)
    {
        var user = await _userRepository.DeleteById(id, cancellationToken);

        if (user is null)
        {
            return Result<User>.Failed(string.Format(ResponseStringConstants.NotFoundResponseStringTemplate,
                nameof(User), nameof(id).ToUpper(), id), ResultType.NotFound);
        }

        await _distributedCache.RemoveAsync(string.Format(RedisKeysConstants.EntityWithIdKeyTemplate, id, nameof(User)),
            cancellationToken);

        return Result<User>.Success(user);
    }

    public async Task<Result<User?>> RestoreUserByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var user = await _userRepository.RestoreUserByIdAsync(id, cancellationToken);

        if (user is null)
        {
            return Result<User>.Failed(string.Format(ResponseStringConstants.NotFoundResponseStringTemplate,
                nameof(User),
                nameof(id).ToUpper(), id), ResultType.NotFound);
        }

        return Result<User>.Success(user);
    }
}