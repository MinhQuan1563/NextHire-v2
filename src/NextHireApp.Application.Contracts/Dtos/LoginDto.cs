using System.ComponentModel.DataAnnotations;
using Volo.Abp.Auditing;

namespace NextHireApp.Dtos
{
    /// <summary>
    /// Input đăng nhập bằng username/email + password.
    /// </summary>
    [DisableAuditing] // ẩn toàn bộ field nhạy cảm trong audit log
    public class LoginDto
    {
        /// <summary>Tên đăng nhập hoặc email.</summary>
        [Required]
        public string UserNameOrEmail { get; set; } = default!;

        /// <summary>Mật khẩu người dùng.</summary>
        [Required]
        [StringLength(128, MinimumLength = 6)]
        public string Password { get; set; } = default!;
    }
}
