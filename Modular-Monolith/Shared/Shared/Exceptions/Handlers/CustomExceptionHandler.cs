using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Shared.Exceptions.Handlers
{
    public class CustomExceptionHandler(ILogger<CustomExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            logger.LogError(exception, "An error occurred while processing the request. Exception: {Message}, Occured at {Time}", exception.Message, DateTime.UtcNow);

            var (statusCode, title, detail) = exception switch
            {
                ValidationException validationException => (StatusCodes.Status400BadRequest, "Validation error", validationException.Message),
                BadRequestException badRequestException => (StatusCodes.Status400BadRequest, badRequestException.Message, badRequestException.Details),
                NotFoundException notFoundException => (StatusCodes.Status404NotFound, "The specified resource was not found", notFoundException.Message),
                InternalServerException internalServerException => (StatusCodes.Status500InternalServerError, internalServerException.Message, internalServerException.Details),
                _ => (StatusCodes.Status500InternalServerError, "An internal server error occurred", exception.Message)
            };

            var problemDetails = new ProblemDetails
            {
                Title = title,
                Detail = detail,
                Status = statusCode,
                Instance = httpContext.Request.Path
            };

            problemDetails.Extensions.Add("traceId", httpContext.TraceIdentifier);

            if (exception is ValidationException validationEx)
            {
                problemDetails.Extensions.Add("errors", validationEx.Errors.Select(e => e.ErrorMessage));
            }

            httpContext.Response.StatusCode = statusCode;

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
    }
}
