using System;

namespace NextHireApp.Auth.Dtos
{
    public class TokenResponseDto
    {
        public string AccessToken { get; set; }
        public DateTime AccessTokenExpiresAt { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiresAt { get; set; }
    }
}
