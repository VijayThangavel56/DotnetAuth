using ECommerce.DTO;
using System.Net;

namespace ECommerce.Shared.Helper
{
    public static class ApiResponseHelper
    {
        public static ApiResponseDto Ok(object? data = null)
        {
            return Success(HttpStatusCode.OK, null, data);
        }

        public static ApiResponseDto Created(object? data = null)
        {
            return Success(HttpStatusCode.Created, null, data);
        }
        public static ApiResponseDto NoContent()
        {
            return Success(HttpStatusCode.NoContent, null, null);
        }

        public static ApiResponseDto Success(HttpStatusCode statusCode, List<ApiMessage>? messages = null, object? result = null)
        {
            messages ??= [];
            return new ApiResponseDto
            {
                Success = true,
                StatusCode = statusCode,
                Messages = messages,
                Result = result
            };
        }
        public static ApiResponseDto NotFound(List<ApiMessage>? messages = null, object? result = null)
        {
            return Failure(HttpStatusCode.NotFound, messages, result);
        }

        public static ApiResponseDto BadRequest(List<ApiMessage>? messages = null, object? result = null)
        {
            return Failure(HttpStatusCode.BadRequest, messages, result);
        }

        public static ApiResponseDto BadRequest(string errorCode, string localizedErrorDesc)
        {
            return Failure(HttpStatusCode.BadRequest, [new ApiMessage { Code = errorCode, Message = localizedErrorDesc }]);
        }

        public static ApiResponseDto InternalServerError(List<ApiMessage>? messages = null, object? result = null)
        {
            return Failure(HttpStatusCode.InternalServerError, messages, result);
        }

        public static ApiResponseDto Unauthorized(List<ApiMessage>? messages = null, object? result = null)
        {
            return Failure(HttpStatusCode.Unauthorized, messages, result);
        }

        public static ApiResponseDto Failure(HttpStatusCode statusCode, List<ApiMessage>? messages = null, object? result = null)
        {
            messages ??= [];

            return new ApiResponseDto
            {
                Success = false,
                StatusCode = statusCode,
                Messages = messages,
                Result = result
            };
        }
    }
}
