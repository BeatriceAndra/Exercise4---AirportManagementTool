using AirportTool.Application.DTOs.Responses;
using AirportTool.Application.Exceptions;
using System.Net;
using System.Text.Json;

namespace AirportManagement.WebApi.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
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
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpStatusCode status;
            string message = exception.Message;
            IEnumerable<string>? details = null;

            switch (exception)
            {
                case NotFoundException:
                    status = HttpStatusCode.NotFound;
                    break;
                case BadRequestException:
                    status = HttpStatusCode.BadRequest;
                    break;
                case GateOverlapException:
                    status = HttpStatusCode.Conflict;
                    break;
                default:
                    status = HttpStatusCode.InternalServerError;
                    _logger.LogError(exception, "Unhandled exception occurred.");
                    break;
            }

            var response = new ErrorResponseDto(
                message: message,
                statusCode: (int)status,
                details: details
            );

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)status;

            var json = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(json);
        }
    }
}
