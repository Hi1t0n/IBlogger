﻿using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using UserService.Domain.Contacts;

namespace UserService.Host.Middlewares;

public class ExceptionMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception) when (exception is TaskCanceledException || exception is TimeoutException)
        {
            var problemDetails = new ProblemDetails
            {
                Title = "Request Timeout",
                Detail = exception.Message,
                Status = StatusCodes.Status408RequestTimeout
            };

            Log.Error("Exception: {Title} - {Detail}", problemDetails.Title, problemDetails.Detail);

            await ModifyHeader(context, problemDetails);
        }
        catch (Exception exception)
        {
            var problemDetails = new ProblemDetails
            {
                Title = "Servet Internal Error",
                Detail = exception.Message,
                Status = StatusCodes.Status500InternalServerError
            };

            Log.Error("Exception: {Title} - {Detail} - {Status}", problemDetails.Title, problemDetails.Detail,
                problemDetails.Status);
            
            await ModifyHeader(context, problemDetails);
        }
    }

    private static async Task ModifyHeader(HttpContext context, ProblemDetails problemDetails)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;
        await context.Response.WriteAsJsonAsync(problemDetails, CancellationToken.None);
    }
}

public static class ExceptionMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder applicationBuilder)
    {
        return applicationBuilder.UseMiddleware<ExceptionMiddleware>();
    }
}