using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NextHireApp.Auth.Dtos;
using NextHireApp.Auth.Interfaces;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;

namespace NextHireApp.Auth.Services
{
    public class TokenService : ITokenService, ITransientDependency
    {
        private readonly IConfiguration _cfg;
        private readonly IDistributedCache _cache;

        private const string RefreshPrefix = "jwt:refresh:"; // key = refreshToken -> userId

        public TokenService(IConfiguration cfg, IDistributedCache cache)
        { 
            _cfg = cfg; _cache = cache; 
        }

        public async Task<TokenResponseDto> IssueAsync(IdentityUser user, string? oldRefreshToken = null)
        {
            // Access token (JWT)
            var securityKey = _cfg["AuthServer:SecurityKey"];
            if (string.IsNullOrWhiteSpace(securityKey))
            {
                throw new InvalidOperationException("The security key is not configured or is empty.");
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var jti = Guid.NewGuid().ToString("N");

            var expires = DateTime.UtcNow.AddMinutes(int.Parse(_cfg["AuthServer:AccessTokenMinutes"]!));
            var token = new JwtSecurityToken(
                issuer: _cfg["AuthServer:Issuer"],
                audience: _cfg["AuthServer:Audience"],
                claims: new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName ?? ""),
                    new Claim(JwtRegisteredClaimNames.Jti, jti)
                },
                expires: expires, signingCredentials: creds);

            var access = new JwtSecurityTokenHandler().WriteToken(token);

            // Refresh token (random) lưu Redis
            if (!string.IsNullOrWhiteSpace(oldRefreshToken))
            {
                // vô hiệu hoá refresh cũ
                await _cache.RemoveAsync(RefreshPrefix + oldRefreshToken);
            }

            var refresh = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            var refreshDays = int.Parse(_cfg["AuthServer:RefreshTokenDays"]!);
            var refreshTtl = TimeSpan.FromDays(refreshDays);

            await _cache.SetStringAsync(RefreshPrefix + refresh, user.Id.ToString(),
                new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = refreshTtl });

            return new TokenResponseDto
            {
                AccessToken = access,
                AccessTokenExpiresAt = expires,
                RefreshToken = refresh,
                RefreshTokenExpiresAt = DateTime.UtcNow.Add(refreshTtl)
            };
        }
    }
}
