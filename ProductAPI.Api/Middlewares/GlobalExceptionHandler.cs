using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.Domain.Exceptions;

namespace ProductAPI.Api.Middlewares;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Ha ocurrido un error no controlado.");

        // Por defecto, tratamos el error como 500 (Internal Server Error)
        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Error Interno del Servidor",
            Detail = "Ocurrió un error inesperado al procesar la solicitud."
        };

        // Reglas de negocio rotas -> 400 Bad Request o 409 Conflict
        if (exception is DomainException domainException)
        {
            problemDetails.Status = StatusCodes.Status400BadRequest;
            problemDetails.Title = "Error de regla de negocio";
            problemDetails.Detail = domainException.Message;
        }

        // Recurso no encontrado -> 404 Not Found
        if (exception is NotFoundException notFoundException)
        {
            problemDetails.Status = StatusCodes.Status404NotFound;
            problemDetails.Title = "Recurso no encontrado";
            problemDetails.Detail = notFoundException.Message;
        }

        // Si la excepción viene de FluentValidation
        if (exception is FluentValidation.ValidationException validationException)
        {
            problemDetails.Status = StatusCodes.Status400BadRequest;
            problemDetails.Title = "Error de validación de datos de entrada";
            problemDetails.Detail = "Uno o más campos tienen errores de validación.";
            problemDetails.Extensions["errors"] = validationException.Errors
                .Select(e => new { e.PropertyName, e.ErrorMessage });
        }

        httpContext.Response.StatusCode = problemDetails.Status.Value;

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true; // Indicamos que ya manejamos el error
    }
}
