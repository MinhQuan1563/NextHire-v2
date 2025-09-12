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
    /// </summary>
    public interface IAuthAppService : IApplicationService
    {
        /// <summary>
        /// Tạo user mới.
        /// </summary>
        Task<TokenResponseDto> RegisterAsync(RegisterDto input);

        /// <summary>
        /// Đăng nhập và nhận JWT + RefreshToken.
        /// </summary>
        Task<TokenResponseDto> LoginAsync(LoginDto input);

        /// <summary>Làm mới AccessToken bằng RefreshToken.</summary>
        Task<TokenResponseDto> RefreshAsync(string refreshToken);

        /// <summary>
        /// Gửi mã đặt lại mật khẩu (reset code) đến email của user.
        /// </summary>
        Task SendResetCodeAsync(string email);

        /// <summary>
        /// Đặt lại mật khẩu bằng reset token đã gửi tới email.
        /// </summary>
        Task ResetAsync(string userId, string resetToken, string newPassword);

        /// <summary>
        /// Đổi mật khẩu khi user đã đăng nhập (cần nhập mật khẩu cũ).
        /// </summary>
        Task ChangeAsync(string currentPassword, string newPassword);
    }
}
