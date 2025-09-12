using System.ComponentModel.DataAnnotations;
using Volo.Abp.Auditing;

namespace NextHireApp.Dtos
{
    public class TokenResponseDto
    {
        public string AccessToken { get; set; } = default!;

        public string RefreshToken { get; set; } = default!;

        public long ExpiresIn { get; set; }
    }
}
