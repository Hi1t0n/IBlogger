using System.Net;
using BaseLibrary.Classes.Contracts;
using BaseLibrary.Classes.Result;
using UserService.Domain.Contacts;
using UserService.Domain.Interfaces;
using UserService.Domain.Models;
using UserService.Infrastructure.Services;

namespace UserService.Host.Endpoints;

/// <summary>
/// Эндпоинты для <see cref="User"/>.
/// </summary>
public static class UserEndpoints
{
    /// <summary>
    /// Добавление URl.
    /// </summary>
    /// <param name="webApplication"><see cref="WebApplication"/>.</param>
    /// <returns>Модифицированный <see cref="WebApplication"/>.</returns>
    public static WebApplication AddUserEndpoints(this WebApplication webApplication)
    {
        var mapGroup = webApplication.MapGroup("/api/users/");

        mapGroup.MapPost(pattern: "/", handler: AddUserAsync);
        mapGroup.MapGet(pattern: "/{userId:guid}", handler: GetUserByIdAsync);
        mapGroup.MapGet(pattern: "/", handler: GetUsersAsync);
        mapGroup.MapPut(pattern: "/{userId:guid}", handler: UpdateUserByIdAsync);
        mapGroup.MapPut(pattern: "/restore/{userId:guid}", handler: RestoreUserByIdAsync);
        mapGroup.MapDelete(pattern: "/{userId:guid}", handler: DeleteUserByIdAsync);

        return webApplication;
    }

    /// <summary>
    /// Добавление пользователя.
    /// </summary>
    /// <param name="contract">Данные пользователя.</param>
    /// <param name="userService"><see cref="IUserService"/>.</param>
    /// <param name="repository"><see cref="IUserRepository"/>.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
    /// <returns>Результат добавления с данными.</returns>
    private static async Task<IResult> AddUserAsync(AddUserRequest contract,
        IUserService userService,
        IUserRepository repository,
        CancellationToken cancellationToken)
    {
        var validateResult = await contract.ValidateData(repository);

        if (!validateResult.IsValid)
        {
            return validateResult.StatusCode switch
            {
                HttpStatusCode.BadRequest => 
                    Results.BadRequest(new Response((int)HttpStatusCode.BadRequest, validateResult.Message)),
                HttpStatusCode.Conflict => 
                    Results.Conflict(new Response((int)HttpStatusCode.Conflict, validateResult.Message)),
                _ => Results.InternalServerError()
            };
        }

        var result = await userService.AddUserAsync(contract, cancellationToken);

        if (!result.IsSuccess)
        {
            return result.ResultType switch
            {
                ResultType.BadRequest => 
                    Results.BadRequest(new Response((int)HttpStatusCode.BadRequest, result.Message)),
                _ => Results.InternalServerError()
            };
        }

        return Results.Ok();
    }

    /// <summary>
    /// Получение пользователя по <paramref name="userId"/>.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="userService"><see cref="IUserService"/>.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
    /// <returns>Результат получения с данными.</returns>
    private static async Task<IResult> GetUserByIdAsync(Guid userId,
        IUserService userService,
        CancellationToken cancellationToken)
    {
        var result = await userService.GetUserByIdAsync(userId, cancellationToken);

        if (!result.IsSuccess)
        {
            return result.ResultType switch
            {
                ResultType.NotFound => 
                    Results.NotFound(new Response((int)HttpStatusCode.NotFound, result.Message)),
                ResultType.BadRequest => 
                    Results.BadRequest(new Response((int)HttpStatusCode.BadRequest, result.Message)),
                _ => Results.InternalServerError()
            };
        }

        return Results.Ok(result.Value);
    }

    /// <summary>
    /// Получение пользователей.
    /// </summary>
    /// <param name="userService"><see cref="IUserService"/>.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
    /// <returns>Список всех пользователей.</returns>
    private static async Task<IResult> GetUsersAsync(IUserService userService, CancellationToken cancellationToken)
    {
        var result = await userService.GetUsersAsync(cancellationToken);

        return Results.Ok(result.Value);
    }

    /// <summary>
    /// Обновление пользователя по Id.
    /// </summary>
    /// <param name="request">Новые данные.</param>
    /// <param name="userService"><see cref="IUserService"/>.</param>
    /// <param name="repository"></param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
    /// <returns>Результат обновления с данными.</returns>
    private static async Task<IResult> UpdateUserByIdAsync(UpdateUserRequest request,
        IUserService userService,
        IUserRepository repository,
        CancellationToken cancellationToken)
    {
        var validateResult = await request.ValidateData(repository);

        if (!validateResult.IsValid)
        {
            return validateResult.StatusCode switch
            {
                HttpStatusCode.BadRequest => 
                    Results.BadRequest(new Response((int)HttpStatusCode.BadRequest, validateResult.Message)),
                HttpStatusCode.Conflict => 
                    Results.Conflict(new Response((int)HttpStatusCode.Conflict, validateResult.Message)),
                _ => Results.InternalServerError()
            };
        }

        var result = await userService.UpdateUserByIdAsync(request, cancellationToken);

        if (!result.IsSuccess)
        {
            return result.ResultType switch
            {
                ResultType.NotFound => 
                    Results.NotFound(new Response((int)HttpStatusCode.NotFound, result.Message)),
                ResultType.BadRequest => 
                    Results.BadRequest(new Response((int)HttpStatusCode.BadRequest, result.Message)),
                _ => Results.InternalServerError()
            };
        }

        return Results.Ok();
    }

    /// <summary>
    /// Удаление пользователя по <see cref="userId"/>.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="userService"><see cref="IUserService"/>.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
    /// <returns>Результат обновления с данными.</returns>
    private static async Task<IResult> DeleteUserByIdAsync(Guid userId,
        IUserService userService,
        CancellationToken cancellationToken)
    {
        var result = await userService.DeleteUserByIdAsync(userId, cancellationToken);

        if (!result.IsSuccess)
        {
            return result.ResultType switch
            {
                ResultType.NotFound => 
                    Results.NotFound(new Response((int)HttpStatusCode.NotFound, result.Message)),
                ResultType.BadRequest => 
                    Results.BadRequest(new Response((int)HttpStatusCode.BadRequest, result.Message)),
                _ => Results.InternalServerError()
            };
        }

        return Results.Ok();
    }

    /// <summary>
    /// Восстановление пользователя по <paramref name="userId"/>.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="userService"><see cref="IUserService"/>.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
    /// <returns>Результат восстановления с данными.</returns>
    private static async Task<IResult> RestoreUserByIdAsync(Guid userId,
        IUserService userService,
        CancellationToken cancellationToken)
    {
        var result = await userService.RestoreUserByIdAsync(userId, cancellationToken);

        if (!result.IsSuccess)
        {
            return result.ResultType switch
            {
                ResultType.NotFound => 
                    Results.NotFound(new Response((int)HttpStatusCode.NotFound, result.Message)),
                ResultType.BadRequest => 
                    Results.BadRequest(new Response((int)HttpStatusCode.BadRequest, result.Message)),
                _ => Results.InternalServerError()
            };
        }

        return Results.Ok();
    }
}