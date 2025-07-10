using ECommerce.DTO;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace ECommerce.API.ServiceExtensions
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
            var response = new ApiResponseDto()
            {
                Success = false,
                Messages =
                [
                    new ApiMessage
                    {
                        Message = exception.Message,

                    }
                ],

            };


            switch (exception)
            {
                case BadHttpRequestException:
                    response.StatusCode = (HttpStatusCode)StatusCodes.Status400BadRequest;
                    response.Title = exception.GetType().Name;
                    break;
                case UnauthorizedAccessException:
                    response.StatusCode = (HttpStatusCode)StatusCodes.Status401Unauthorized;
                    response.Title = exception.GetType().Name;
                    break;
                case NotImplementedException:
                    response.StatusCode = (HttpStatusCode)StatusCodes.Status501NotImplemented;
                    response.Title = exception.GetType().Name;
                    break;
                default:
                    response.StatusCode = (HttpStatusCode)StatusCodes.Status500InternalServerError;
                    response.Title = "Internal Server Error";
                    break;
            }
            httpContext.Response.StatusCode =(int)response.StatusCode;
            await httpContext.Response.WriteAsJsonAsync(response, cancellationToken: cancellationToken);
            return true;
        }
    }
}
