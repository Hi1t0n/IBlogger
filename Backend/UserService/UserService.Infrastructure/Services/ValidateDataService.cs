using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using UserService.Domain;
using UserService.Domain.Contacts;
using UserService.Domain.Interfaces;

namespace UserService.Infrastructure.Services;

/// <summary>
/// Сервис валидации данных.
/// </summary>
public static class ValidateDataService
{
    /// <summary>
    /// Валидация данных для <see cref="AddUserRequest"/>.
    /// </summary>
    /// <param name="contract">Данные для валидации.</param>
    /// <param name="userRepository"><see cref="IUserRepository"/>.</param>
    /// <returns>Результат валидации.</returns>
    public static async Task<ValidateResult> ValidateData(this AddUserRequest contract, IUserRepository userRepository)
    {
        if(string.IsNullOrWhiteSpace(contract.UserName))
        {
            return ValidateResult.Invalid(HttpStatusCode.BadRequest, $"Incorrect Username");
        }

        if (await userRepository.ExistByUserName(contract.UserName.ToLower()))
        {
            return  ValidateResult.Invalid(HttpStatusCode.BadRequest, $"Username is already taken");
        }
        
        if(string.IsNullOrWhiteSpace(contract.Password))
        {
            return ValidateResult.Invalid(HttpStatusCode.BadRequest, $"Incorrect Password");
        }

        if(!IsValidEmail(contract.Email))
        {
            return ValidateResult.Invalid(HttpStatusCode.BadRequest, $"Incorrect Email");
        }
        
        if (await userRepository.ExistByEmail(contract.Email!.ToLower()))
        {
            return ValidateResult.Invalid(HttpStatusCode.Conflict, $"Email is already taken");
        }

        if (!IsValidPhoneNumber(contract.PhoneNumber))
        {
            return ValidateResult.Invalid(HttpStatusCode.BadRequest, $"Incorrect phone number");
        }
        
        if (await userRepository.ExistByPhoneNumber(contract.PhoneNumber))
        {
            return ValidateResult.Invalid(HttpStatusCode.Conflict, $"Phone number is already taken");
        }
        
        
        return ValidateResult.Valid();
    }
    
    /// <summary>
    /// Валидация данных для <see cref="UpdateUserRequest"/>.
    /// </summary>
    /// <param name="contract">Данные для валидации.</param>
    /// <param name="userRepository"><see cref="IUserRepository"/>.</param>
    /// <returns>Результат валидации.</returns>
    public static async Task<ValidateResult> ValidateData(this UpdateUserRequest contract, IUserRepository userRepository)
    {
        if(string.IsNullOrWhiteSpace(contract.UserName))
        {
            return ValidateResult.Invalid(HttpStatusCode.BadRequest, $"Incorrect Username");
        }

        if (await userRepository.ExistByUserName(contract.UserName.ToLower()))
        {
            return  ValidateResult.Invalid(HttpStatusCode.BadRequest, $"Username is already taken");
        }

        if(!IsValidEmail(contract.Email))
        {
            return ValidateResult.Invalid(HttpStatusCode.BadRequest, $"Incorrect Email");
        }
        
        if (await userRepository.ExistByEmail(contract.Email!.ToLower()))
        {
            return ValidateResult.Invalid(HttpStatusCode.Conflict, $"Email is already taken");
        }

        if (!IsValidPhoneNumber(contract.PhoneNumber))
        {
            return ValidateResult.Invalid(HttpStatusCode.BadRequest, $"Incorrect phone number");
        }
        
        if (await userRepository.ExistByPhoneNumber(contract.PhoneNumber))
        {
            return ValidateResult.Invalid(HttpStatusCode.Conflict, $"Phone number is already taken");
        }
        
        
        return ValidateResult.Valid();
    }
    
    /// <summary>
    /// Проверка email на корректность в соответствии с RFC 5322
    /// </summary>
    /// <param name="email">Электронная почта</param>
    /// <returns>Результат проверки</returns>
    private static bool IsValidEmail(string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return false;
        }

        try
        {
            var mailAddress = new MailAddress(email);
            return true;
        }
        catch (Exception exception)
        {
            return false;
        }
    }

    /// <summary>
    /// Валидация номера телефона.
    /// </summary>
    /// <param name="phoneNumber">Номер телефона.</param>
    /// <returns>True - если телефон соответствует формату, иначе false.</returns>
    private static bool IsValidPhoneNumber(string? phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
        {
            return false;
        }
        
        string phoneRegex = @"^\+?[1-9]\d{0,2}[-.\s]?(\(?\d{1,4}\)?[-.\s]?)*\d{4,10}$";
        return Regex.IsMatch(phoneNumber, phoneRegex);
    }
}