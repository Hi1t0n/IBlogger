using System.Net;
using BaseLibrary.Classes.Result;
using UserService.Domain.Contacts;
using UserService.Domain.Interfaces;
using UserService.Domain.Models;
using UserService.Infrastructure.Extensions;
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
        mapGroup.MapDelete(pattern: "/{userId:guid}", handler: DeleteUserByIdAsync);
        mapGroup.MapPut(pattern: "/restore/{userId:guid}", handler: RestoreUserByIdAsync);

        return webApplication;
    }

    /// <summary>
    /// Добавление пользователя.
    /// </summary>
    /// <param name="contract">Данные пользователя.</param>
    /// <param name="repository"><see cref="IUserRepository"/>.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
    /// <returns>Результат добавления с данными.</returns>
    private static async Task<IResult> AddUserAsync(AddUserRequestContract contract,
        IUserRepository repository,
        CancellationToken cancellationToken)
    {
        var validateResult = await contract.ValidateData(repository);

        if (!validateResult.IsValid)
        {
            switch (validateResult.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    return Results.BadRequest
                        (new Response((int)HttpStatusCode.BadRequest, validateResult.Message));
                case HttpStatusCode.Conflict:
                    return Results.Conflict
                        (new Response((int)HttpStatusCode.Conflict, validateResult.Message));
            }
        }

        var user = contract.ToModel();
        await repository.Create(user, cancellationToken);

        return Results.Ok();
    }

    /// <summary>
    /// Получение пользователя по <paramref name="userId"/>.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="repository"><see cref="IUserRepository"/>.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
    /// <returns>Результат получения с данными.</returns>
    private static async Task<IResult> GetUserByIdAsync(Guid userId,
        IUserRepository repository,
        CancellationToken cancellationToken)
    {
        var result = await repository.GetById(userId, cancellationToken);

        if (!result.IsSuccess)
        {
            switch (result.ResultType)
            {
                case ResultType.NotFound:
                    return Results.NotFound(new Response((int)HttpStatusCode.NotFound, result.Message));

                case ResultType.BadRequest:
                    return Results.BadRequest(new Response((int)HttpStatusCode.BadRequest, result.Message));
            }
        }

        var response = result.Value!.ToResponse();

        return Results.Ok(response);
    }

    /// <summary>
    /// Получение пользователей.
    /// </summary>
    /// <param name="repository"><see cref="IUserRepository"/>.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
    /// <returns>Список всех пользователей.</returns>
    private static async Task<IResult> GetUsersAsync(IUserRepository repository, CancellationToken cancellationToken)
    {
        var result = await repository.Get(cancellationToken);
        var response = result!.Select(x => x.ToResponse());

        return Results.Ok(response);
    }

    /// <summary>
    /// Обновление пользователя по <paramref name="userId"/>.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="contract">Новые данные.</param>
    /// <param name="repository"><see cref="IUserRepository"/>.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
    /// <returns>Результат обновления с данными.</returns>
    private static async Task<IResult> UpdateUserByIdAsync(Guid userId,
        UpdateUserRequestContract contract,
        IUserRepository repository,
        CancellationToken cancellationToken)
    {
        var validateResult = await contract.ValidateData(repository);

        if (!validateResult.IsValid)
        {
            switch (validateResult.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    return Results.BadRequest
                        (new Response((int)HttpStatusCode.BadRequest, validateResult.Message));
                case HttpStatusCode.Conflict:
                    return Results.Conflict
                        (new Response((int)HttpStatusCode.Conflict, validateResult.Message));
            }
        }

        var user = contract.ToModel();
        var result = await repository.UpdateById(userId, user, cancellationToken);

        if (!result.IsSuccess)
        {
            switch (result.ResultType)
            {
                case ResultType.NotFound:
                    return Results.NotFound(new Response((int)HttpStatusCode.NotFound, result.Message));

                case ResultType.BadRequest:
                    return Results.BadRequest(new Response((int)HttpStatusCode.BadRequest, result.Message));
            }
        }

        return Results.Ok();
    }

    /// <summary>
    /// Удаление пользователя по <see cref="userId"/>.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="repository"><see cref="IUserRepository"/>.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
    /// <returns>Результат обновления с данными.</returns>
    private static async Task<IResult> DeleteUserByIdAsync(Guid userId,
        IUserRepository repository,
        CancellationToken cancellationToken)
    {
        var result = await repository.DeleteById(userId, cancellationToken);

        if (!result.IsSuccess)
        {
            switch (result.ResultType)
            {
                case ResultType.NotFound:
                    return Results.NotFound(new Response((int)HttpStatusCode.NotFound, result.Message));

                case ResultType.BadRequest:
                    return Results.BadRequest(new Response((int)HttpStatusCode.BadRequest, result.Message));
            }
        }

        return Results.Ok();
    }

    /// <summary>
    /// Восстановление пользователя по <paramref name="userId"/>.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="repository"><see cref="IUserRepository"/>.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
    /// <returns>Результат восстановления с данными.</returns>
    private static async Task<IResult> RestoreUserByIdAsync(Guid userId,
        IUserRepository repository,
        CancellationToken cancellationToken)
    {
        var result = await repository.RestoreUserByIdAsync(userId, cancellationToken);

        if (result is null)
        {
            return Results.NotFound
                (new Response((int)HttpStatusCode.NotFound, $"User with Id: {userId} Not Found"));
        }

        return Results.Ok();
    }
}