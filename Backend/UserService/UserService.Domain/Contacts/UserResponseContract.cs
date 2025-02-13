namespace UserService.Domain.Contacts;

public record UserResponseContract(
    Guid UserId,
    string UserName,
    string RoleName,
    string? Email,
    bool? EmailConfirmed,
    string? PhoneNumber,
    bool? PhoneNumberConfirmed);