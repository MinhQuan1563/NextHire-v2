using Microsoft.AspNetCore.Authorization;
using NextHireApp.Dtos;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Auditing;

namespace NextHireApp.Service
{
    /// <summary>
    /// Dịch vụ xác thực: đăng ký, đăng nhập, reset/đổi mật khẩu.
    /// Method nào cho phép khách vãng lai thì đánh [AllowAnonymous].
    /// </summary>
    public interface IAuthAppService : IApplicationService
    {
        /// <summary>Tạo user mới.</summary>
        [AllowAnonymous]
        Task RegisterAsync(RegisterDto input);

        /// <summary>Đăng nhập và nhận JWT + RefreshToken.</summary>
        [AllowAnonymous]
        [DisableAuditing] // không log input/output
        Task<TokenResponseDto> PasswordLoginAsync(LoginDto input);

        /// <summary>Gửi email reset mật khẩu.</summary>
        [AllowAnonymous]
        Task RequestPasswordResetAsync([Required, EmailAddress] string email);

        /// <summary>Đổi mật khẩu (yêu cầu đã đăng nhập).</summary>
        [Authorize]
        Task ChangePasswordAsync([Required] string current, [Required] string @new);
    }
}
