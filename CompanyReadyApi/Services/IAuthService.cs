using CompanyReadyApi.DTOs;

namespace CompanyReadyApi.Services
{
    public interface IAuthService
    {
        Task<bool> RegisterAsync(RegisterRequestDto dto);
        Task<AuthResponseDto?> LoginAsync(LoginRequestDto dto);
    }
}