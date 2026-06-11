using Microsoft.AspNetCore.Diagnostics;
using SMB.API.Contracts;
using SMB.APPLICATION.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace SMB.API.Middleware;

public class ExceptionHandler(ILogger<ExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var (statusCode, message) = exception switch
        {
            APPLICATION.Exceptions.ValidationException =>
                (StatusCodes.Status400BadRequest, exception.Message),

            DuplicateResourceException =>
                (StatusCodes.Status409Conflict, exception.Message),

            InvalidCredentialsException =>
                (StatusCodes.Status401Unauthorized, exception.Message),

            ForbiddenException =>
                (StatusCodes.Status403Forbidden, exception.Message),

            ResourceNotFoundException =>
                (StatusCodes.Status404NotFound, exception.Message),

            _ =>
                (StatusCodes.Status500InternalServerError, "Ocurrió un error inesperado")
        };
        
        if (statusCode == StatusCodes.Status500InternalServerError)
        {
            logger.LogError(exception, "Error inesperado");
        }
        else
        {
            logger.LogWarning(exception, "Error controlado");
        }
        
        httpContext.Response.StatusCode = statusCode;
        
        await httpContext.Response.WriteAsJsonAsync(new Answer<object?>
        {
            Message = message,
            Response = null,
            Code = statusCode
        }, cancellationToken);

        return true;
    }
}