using System.ComponentModel.DataAnnotations;
using Volo.Abp.Auditing;
using Volo.Abp.Identity;
using Volo.Abp.Validation;

namespace NextHireApp.Dtos
{
    /// <summary>
    /// Đăng ký
    /// </summary>
    public class RegisterDto
    {
        /// <summary>Tên đăng nhập (unique).</summary>
        [Required]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxUserNameLength))]
        public string UserName { get; set; } = default!;

        /// <summary>Email dùng để kích hoạt/khôi phục.</summary>
        [Required]
        [EmailAddress]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxEmailLength))]
        public string Email { get; set; } = default!;

        /// <summary>Mật khẩu người dùng.</summary>
        [Required]
        [StringLength(128, MinimumLength = 6)]
        [DisableAuditing] // không ghi log giá trị nhạy cảm
        public string Password { get; set; } = default!;
    }
}
