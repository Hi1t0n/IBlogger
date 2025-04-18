using UserService.Domain.Contacts;
using UserService.Domain.Models;
using UserService.Infrastructure.Services;

namespace UserService.Infrastructure.Mappings;

public static class UserMapping
{
    public static UserResponse ToResponse(this User user)
    {
        return new UserResponse(user.Id, user.UserName!, user.Role?.RoleName!, user.Email, user.EmailConfirmed,
            user.PhoneNumber, user.PhoneNumberConfirmed, user.CreatedOn, user.ModifiedOn);
    }

    public static List<UserResponse>? ToResponse(this List<User> users)
    {
        return users
            .Select(x => x.ToResponse())
            .ToList();
    }
    
    public static User ToModel(this AddUserRequest request)
    {
        return new User()
        {
            Id = Guid.NewGuid(),
            UserName = request.UserName.ToLower(),
            Password = CryptoService.HashPassword(request.Password),
            Email = request.Email!.ToLower(),
            PhoneNumber = request.PhoneNumber,
            CreatedOn = DateTime.UtcNow,
            ModifiedOn = DateTime.UtcNow
        };
    }

    public static User ToModel(this UpdateUserRequest request)
    {
        return new User()
        {
            UserName = request.UserName.ToLower(),
            Email = request.Email.ToLower(),
            PhoneNumber = request.PhoneNumber
        };
    }
}