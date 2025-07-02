using DotnetAuth.Domain.Contracts;
using Microsoft.AspNetCore.Diagnostics;

namespace DotnetAuth.Exceptions
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;
        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
             _logger.LogError(exception, "An unhandled exception occurred: {Message}", exception.Message);
            var response = new ErrorResponse
            {
                Message = exception.Message,
            };

            switch (exception)
            {
                case BadHttpRequestException:
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    response.Titel=exception.GetType().Name;
                    break;
                case UnauthorizedAccessException:
                    response.StatusCode = StatusCodes.Status401Unauthorized;
                    response.Titel = exception.GetType().Name;
                    break;
                case NotImplementedException:
                    response.StatusCode = StatusCodes.Status501NotImplemented;
                    response.Titel = exception.GetType().Name;
                    break;
                default:
                    response.StatusCode = StatusCodes.Status500InternalServerError;
                    response.Titel = "Internal Server Error";
                    break;
            }
            httpContext.Response.StatusCode = response.StatusCode;
            await httpContext.Response.WriteAsJsonAsync(response, cancellationToken: cancellationToken);
            return true;
        }
    }
}
