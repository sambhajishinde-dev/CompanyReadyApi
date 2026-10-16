using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CompanyReadyApi.Configuration;
using CompanyReadyApi.Data;
using CompanyReadyApi.DTOs;
using CompanyReadyApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CompanyReadyApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _db;
        private readonly IPasswordHasher<AppUser> _passwordHasher;
        private readonly JwtSettings _jwt;

        public AuthService(
            ApplicationDbContext db,
            IPasswordHasher<AppUser> passwordHasher,
            IOptions<JwtSettings> jwtOptions)
        {
            _db = db;
            _passwordHasher = passwordHasher;
            _jwt = jwtOptions.Value;
        }

        public async Task<bool> RegisterAsync(RegisterRequestDto dto)
        {
            var email = dto.Email.Trim().ToLowerInvariant();

            var exists = await _db.Users
                .AnyAsync(u => u.Email == email);

            if (exists)
                return false;

            var user = new AppUser
            {
                Email = email,
                Role = "User"
            };

            user.PasswordHash = _passwordHasher.HashPassword(
                user, dto.Password);

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<AuthResponseDto?> LoginAsync(LoginRequestDto dto)
        {
            var email = dto.Email.Trim().ToLowerInvariant();

            var user = await _db.Users
                .SingleOrDefaultAsync(u => u.Email == email);

            if (user == null)
                return null;

            var verification = _passwordHasher.VerifyHashedPassword(
                user, user.PasswordHash, dto.Password);

            if (verification == PasswordVerificationResult.Failed)
                return null;

            var expiresAt = DateTime.UtcNow.AddMinutes(_jwt.ExpiryMinutes);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(JwtRegisteredClaimNames.Jti,
                    Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwt.Key));

            var credentials = new SigningCredentials(
                key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: expiresAt,
                signingCredentials: credentials);

            return new AuthResponseDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                ExpireAtUtc = expiresAt,
                Email = user.Email,
                Role = user.Role
            };
        }
    }
}