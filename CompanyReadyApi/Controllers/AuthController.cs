using CompanyReadyApi.DTOs;
using CompanyReadyApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CompanyReadyApi.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequestDto dto)
        {
            var registered = await _authService.RegisterAsync(dto);

            if (!registered)
                return Conflict(new { message = "Email is already registered." });

            return Ok(new { message = "Registration successful." });
        }


        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login(
            LoginRequestDto dto)
        {
            var result = await _authService.LoginAsync(dto);

            if (result == null)
                return Unauthorized(new
                {
                    message = "Invalid email or password."
                });

            return Ok(result);
        }
    }
}