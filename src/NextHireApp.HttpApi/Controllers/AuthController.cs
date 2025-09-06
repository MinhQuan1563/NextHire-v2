using Microsoft.AspNetCore.Mvc;
using NextHireApp.Auth;
using NextHireApp.Auth.Dtos;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;

namespace NextHireApp.Controllers
{
    public class AuthController : AbpController
    {
        private readonly IAuthAppService _authService;

        public AuthController(IAuthAppService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task Register(RegisterDto input)
        {
            await _authService.RegisterAsync(input);
        }

        [HttpPost("login")]
        public async Task<TokenResponseDto> Login(LoginDto input)
        {
            return await _authService.LoginAsync(input);
        }

        [HttpPost("refresh")]
        public async Task<TokenResponseDto> Refresh(RefreshRequestDto input)
        {
            return await _authService.RefreshAsync(input);
        }
    }
}
