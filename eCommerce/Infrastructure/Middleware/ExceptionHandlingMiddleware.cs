using FluentValidation;
using System.Net;
using System.Text.Json;

namespace eCommerce.Infrastructure.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleException(context, ex);
            }
        }

        private static async Task HandleException(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            ErrorResponse response = exception switch
            {
                ValidationException validationException => new ErrorResponse(
                    statusCode: (int)HttpStatusCode.BadRequest,
                    message: "Validation failed",
                    errors: validationException.Errors.Select(e => new { e.PropertyName, e.ErrorMessage })
                ),

                KeyNotFoundException => new ErrorResponse(
                    statusCode: (int)HttpStatusCode.NotFound,
                    message: exception.Message
                ),

                InvalidOperationException => new ErrorResponse(
                    statusCode: (int)HttpStatusCode.BadRequest,
                    message: exception.Message
                ),

                _ => new ErrorResponse(
                    statusCode: (int)HttpStatusCode.InternalServerError,
                    //message: "An unexpected error occurred"
                    message: exception.Message
                )
            };

            context.Response.StatusCode = response.StatusCode;

            var json = JsonSerializer.Serialize(response);

            await context.Response.WriteAsync(json);
        }

        private sealed class ErrorResponse
        {
            public int StatusCode { get; }
            public string Message { get; }
            public object? Errors { get; }

            public ErrorResponse(int statusCode, string message, object? errors = null)
            {
                StatusCode = statusCode;
                Message = message;
                Errors = errors;
            }
        }
    }
}