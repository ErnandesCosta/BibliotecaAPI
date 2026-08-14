using BibliotecaAPI.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaAPI.Middleware;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(
        ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(
            exception,
            "Ocorreu uma exceção não tratada.");

        var statusCode = exception switch
        {
            NotFoundException => StatusCodes.Status404NotFound,
            BusinessConflictException =>
                StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };

        var title = statusCode switch
        {
            StatusCodes.Status404NotFound =>
                "Recurso não encontrado",

            StatusCodes.Status409Conflict =>
                "Conflito de negócio",

            _ => "Erro interno do servidor"
        };

        var detail = statusCode ==
                     StatusCodes.Status500InternalServerError
            ? "Ocorreu um erro inesperado no servidor."
            : exception.Message;

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail,
            Instance = httpContext.Request.Path
        };

        httpContext.Response.StatusCode = statusCode;

        await httpContext.Response.WriteAsJsonAsync(
            problemDetails,
            cancellationToken);

        return true;
    }
}