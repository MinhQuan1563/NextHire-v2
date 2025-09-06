using NextHireApp.Auth.Dtos;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace NextHireApp.Auth
{
    public interface IAuthAppService : IApplicationService
    {
        Task RegisterAsync(RegisterDto input);
        Task<TokenResponseDto> LoginAsync(LoginDto input);
        Task<TokenResponseDto> RefreshAsync(RefreshRequestDto input);
        Task LogoutAsync(string accessTokenJti, DateTime accessExpiryUtc);
    }
}
