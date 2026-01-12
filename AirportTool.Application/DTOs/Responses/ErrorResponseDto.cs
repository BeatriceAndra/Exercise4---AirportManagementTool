namespace AirportTool.Application.DTOs.Responses
{
    public class ErrorResponseDto
    {
        public string Message { get; set; } = null!;
        public int? StatusCode { get; set; }
        public IEnumerable<string>? Details { get; set; }

        public ErrorResponseDto(string message, int? statusCode = null, IEnumerable<string>? details = null)
        {
            Message = message;
            StatusCode = statusCode;
            Details = details;
        }
    }
}
