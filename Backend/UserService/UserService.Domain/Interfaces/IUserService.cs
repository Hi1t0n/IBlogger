using BaseLibrary.Classes.Result;
using BaseLibrary.Interfaces;
using UserService.Domain.Contacts;
using UserService.Domain.Models;

namespace UserService.Domain.Interfaces;

public interface IUserService : IService<User>
{
    public Task<Result<User?>> Add(AddUserRequest request, CancellationToken cancellationToken);
    public Task<Result<User?>> UpdateById(UpdateUserRequest request, CancellationToken cancellationToken);
    public Task<Result<User?>> RestoreUserByIdAsync(Guid id, CancellationToken cancellationToken);
}