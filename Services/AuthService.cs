
using ClaimBack.API.DTOs;
using ClaimBack.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ClaimBack.API.Services
{
    public class AuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<(IdentityResult Result, string? Token)> RegisterAsync(RegisterDto dto)
        {
            var user = new ApplicationUser
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                UserName = dto.Email,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                return (result, null);
            }

            var token = GenerateJwtToken(user);

            return (result, token);
        }

        public async Task<string?> LoginAsync(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null)
                return null;

            var passwordValid = await _userManager.CheckPasswordAsync(
                user, dto.Password);

            if (!passwordValid)
                return null;

            return GenerateJwtToken(user);
        }

        private string GenerateJwtToken(ApplicationUser user)
        {
            var jwtKey = _configuration["Jwt:Key"]
                ?? throw new InvalidOperationException("JWT key is missing.");

            var key = Convert.FromBase64String(jwtKey);

            var signingKey = new SymmetricSecurityKey(key);

            var credentials = new SigningCredentials(
                signingKey,
                SecurityAlgorithms.HmacSha256);

            System.Security.Claims.Claim[] claims =
            {
                new System.Security.Claims.Claim(
                    ClaimTypes.NameIdentifier, user.Id),

                new System.Security.Claims.Claim(
                    ClaimTypes.Email, user.Email!),

                new System.Security.Claims.Claim(
                    ClaimTypes.Name,
                    user.FirstName + " " + user.LastName)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
