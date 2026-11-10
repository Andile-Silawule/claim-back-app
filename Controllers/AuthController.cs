
using ClaimBack.API.DTOs;
using ClaimBack.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClaimBack.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var result = await _authService.RegisterAsync(dto);

            if (!result.Succeeded)
            {
                return BadRequest(new
                {
                    Errors = result.Errors.Select(e => e.Description)
                });
            }

            return Ok(new
            {
                Message = "Account registered successfully."
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var token = await _authService.LoginAsync(dto);

            if (token == null)
            {
                return Unauthorized(new
                {
                    Message = "Invalid email or password."
                });
            }

            return Ok(new
            {
                Message = "Login successful.",
                Token = token
            });
        }
    }
}
