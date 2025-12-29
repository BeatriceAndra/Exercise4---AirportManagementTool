namespace AirportTool.Application.DTOs.Auth
{
    public class AuthResponseDto
    {
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Token { get; set; } = null!;
        public IEnumerable<string> Roles { get; set; } = new List<string>();
    }
}
