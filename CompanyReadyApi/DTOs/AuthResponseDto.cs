namespace CompanyReadyApi.DTOs
{
    public class AuthResponseDto
    {

        public string Token { get; set; } = string.Empty;
        public DateTime ExpireAtUtc { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
