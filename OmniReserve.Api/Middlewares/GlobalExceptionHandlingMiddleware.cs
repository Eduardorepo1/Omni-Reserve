using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using OmniReserve.Domain.Exceptions;
using ValidationException = OmniReserve.Application.Common.Exceptions.ValidationException;

namespace OmniReserve.Api.Middlewares;

public class GlobalExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

    public GlobalExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            // Mostrar el error completo, incluyendo la excepción interna.
            _logger.LogError(
                ex,
                "ERROR COMPLETO: {Error}",
                ex.ToString());

            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(
        HttpContext context,
        Exception exception)
    {
        context.Response.ContentType = "application/problem+json";

        ProblemDetails problemDetails;

        if (exception is ValidationException validationEx)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Error de Validación",
                Detail = "Se produjeron uno o más errores de validación."
            };

            problemDetails.Extensions.Add("errors", validationEx.Errors);

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(problemDetails));

            return;
        }
        else if (exception is DomainException domainEx)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Error de Dominio",
                Detail = domainEx.Message
            };

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(problemDetails));

            return;
        }

        context.Response.StatusCode =
            StatusCodes.Status500InternalServerError;

        problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Ha ocurrido un error interno en el servidor.",
            Detail = exception.Message
        };

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(problemDetails));
    }
}