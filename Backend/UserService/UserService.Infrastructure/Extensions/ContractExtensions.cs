using UserService.Domain.Contacts;
using UserService.Domain.Models;
using UserService.Infrastructure.Services;

namespace UserService.Infrastructure.Extensions;

public static class ContractExtensions
{
    public static User ToModel(this AddUserRequest contract)
    {
        return new User()
        {
            Id = Guid.NewGuid(),
            UserName = contract.UserName.ToLower(),
            Password = CryptoService.HashPassword(contract.Password),
            Email = contract.Email!.ToLower(),
            PhoneNumber = contract.PhoneNumber,
            CreatedOn = DateTime.UtcNow,
            ModifiedOn = DateTime.UtcNow
        };
    }

    public static User ToModel(this UpdateUserRequest contract)
    {
        return new User()
        {
            UserName = contract.UserName.ToLower(),
            Email = contract.Email.ToLower(),
            PhoneNumber = contract.PhoneNumber
        };
    }
}