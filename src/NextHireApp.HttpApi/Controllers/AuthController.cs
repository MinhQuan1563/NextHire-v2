using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NextHireApp.Dtos;
using NextHireApp.Service;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NextHireApp.Controllers
{
    [Route("api/[controller]/[action]")]
    public class AuthController : NextHireAppController
    {
        private readonly IAuthAppService _authService;
        public AuthController(IAuthAppService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<TokenResponseDto> Register(RegisterDto input)
        {
            return await _authService.RegisterAsync(input);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<TokenResponseDto> Login(LoginDto input)
        {
            return await _authService.LoginAsync(input);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<TokenResponseDto> Refresh([FromForm] string refreshToken)
        {
            return await _authService.RefreshAsync(refreshToken);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Forgot([FromBody] ForgotPasswordDto input)
        {
            await _authService.SendResetCodeAsync(input.Email);
            return Ok(new { message = "If email exists, a reset token was sent." });
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Reset([FromBody] ResetPasswordDto input)
        {
            await _authService.ResetAsync(input.UserId, input.ResetToken, input.NewPassword);
            return Ok(new { message = "Password has been reset." });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Change([FromBody] ChangePasswordDto input)
        {
            await _authService.ChangeAsync(input.CurrentPassword, input.NewPassword);
            return Ok(new { message = "Password changed." });
        }

        [HttpPost]
        public async Task<IActionResult> Logout([FromServices] ITokenBlacklistService blacklist)
        {
            var jti = User.FindFirstValue("jti");
            var expUnix = User.FindFirst("exp")?.Value;

            if (string.IsNullOrEmpty(jti) || string.IsNullOrEmpty(expUnix))
                return Ok(new { message = "No token to revoke." });

            var exp = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expUnix));
            var ttl = exp - DateTimeOffset.UtcNow;
            if (ttl > TimeSpan.Zero)
            {
                await blacklist.BlacklistAsync(jti, ttl);
            }

            return Ok(new { message = "Logged out." });
        }
    }
}
