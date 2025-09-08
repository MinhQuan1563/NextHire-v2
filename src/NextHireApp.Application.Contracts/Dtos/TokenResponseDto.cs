using System.ComponentModel.DataAnnotations;
using Volo.Abp.Auditing;

namespace NextHireApp.Dtos
{
    /// <summary>
    /// Kết quả trả về sau khi cấp token.
    /// </summary>
    [DisableAuditing] // ẩn toàn bộ field nhạy cảm trong audit log
    public class TokenResponseDto
    {
        /// <summary>JWT access token.</summary>
        [Required]
        public string AccessToken { get; set; } = default!;

        /// <summary>Refresh token để xin token mới khi hết hạn.</summary>
        [Required]
        public string RefreshToken { get; set; } = default!;

        /// <summary>Thời gian sống của access token (giây).</summary>
        public long ExpiresIn { get; set; }
    }
}
