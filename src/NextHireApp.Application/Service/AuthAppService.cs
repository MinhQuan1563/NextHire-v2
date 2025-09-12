using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NextHireApp.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Authorization;
using Volo.Abp.Emailing;
using Volo.Abp.Identity;
using Volo.Abp.Uow;
using Volo.Abp.Users;
using IdentityUser = Volo.Abp.Identity.IdentityUser;


namespace NextHireApp.Service
{
    public class AuthAppService : ApplicationService, IAuthAppService
    {
        private readonly IdentityUserManager _userManager;
        private readonly IHttpClientFactory _httpClientFactory; // inject để call /connect/token
        private readonly IConfiguration _configuration;
        private readonly ICurrentUser _currentUser;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<AuthAppService> _logger;
        private readonly IUnitOfWorkManager _uow;

        public AuthAppService(
            IdentityUserManager userManager,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            ICurrentUser currentUser,
            IEmailSender emailSender,
            IUnitOfWorkManager uow,
            ILogger<AuthAppService> logger)
        {
            _userManager = userManager;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _currentUser = currentUser;
            _emailSender = emailSender;
            _uow = uow;
            _logger = logger;
        }

        public async Task<TokenResponseDto> RegisterAsync(RegisterDto input)
        {
            using (var uow = _uow.Begin(requiresNew: true, isTransactional: true))
            {
                var user = new IdentityUser(GuidGenerator.Create(), input.UserName, input.Email, CurrentTenant.Id)
                {
                    Name = input.FullName
                };

                (await _userManager.CreateAsync(user, input.Password)).CheckErrors();
                (await _userManager.AddDefaultRolesAsync(user)).CheckErrors();

                await uow.CompleteAsync(); // COMMIT lại dữ liệu trước khi gọi LoginAsync
            }

            return await LoginAsync(new LoginDto
            {
                UserNameOrEmail = input.UserName,
                Password = input.Password
            });
        }

        public async Task<TokenResponseDto> LoginAsync(LoginDto input)
        {
            // Proxy sang /connect/token
            var user = await FindByNameOrEmailAsync(input.UserNameOrEmail);
            if (user == null || !await _userManager.CheckPasswordAsync(user, input.Password))
                throw new BusinessException("Auth:InvalidCredentials");

            var client = _httpClientFactory.CreateClient();
            var authority = _configuration["AuthServer:Authority"];
            var clientId = _configuration["AuthServer:PasswordClientId"];
            var clientSecret = _configuration["AuthServer:PasswordClientSecret"];
            var scope = _configuration["AuthServer:Scope"];

            if (authority == null || clientId == null || clientSecret == null || scope == null)
                throw new BusinessException("Auth:ConfigurationMissing");

            var tokenEndpoint = $"{authority}/connect/token";

            var payload = new Dictionary<string, string>
            {
                ["grant_type"] = "password",
                ["client_id"] = clientId,
                ["client_secret"] = clientSecret,
                ["username"] = input.UserNameOrEmail,
                ["password"] = input.Password,
                ["scope"] = scope
            };

            using var req = new HttpRequestMessage(HttpMethod.Post, tokenEndpoint)
            { Content = new FormUrlEncodedContent(payload) };

            using var res = await client.SendAsync(req);
            if (!res.IsSuccessStatusCode)
            {
                var body = await res.Content.ReadAsStringAsync();
                throw new BusinessException("Auth:TokenEndpointFailed")
                    .WithData("Status", (int)res.StatusCode)
                    .WithData("Body", body);
            }

            var dto = await res.Content.ReadFromJsonAsync<TokenEndpointResponse>();
            return new TokenResponseDto
            {
                AccessToken = dto!.access_token,
                RefreshToken = dto.refresh_token,
                ExpiresIn = dto.expires_in
            };
        }

        private async Task<IdentityUser> FindByNameOrEmailAsync(string usernameOrEmail)
        {
            return await _userManager.FindByNameAsync(usernameOrEmail)
                   ?? await _userManager.FindByEmailAsync(usernameOrEmail)
                   ?? throw new BusinessException("Auth:UserNotFound");
        }

        private sealed record TokenEndpointResponse(string access_token, string token_type, long expires_in, string refresh_token);

        public async Task<TokenResponseDto> RefreshAsync(string refreshToken)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
                throw new BusinessException("Auth:InvalidRefreshToken");

            var client = _httpClientFactory.CreateClient();

            var authority = _configuration["AuthServer:Authority"];
            var clientId = _configuration["AuthServer:PasswordClientId"];
            var clientSecret = _configuration["AuthServer:PasswordClientSecret"];

            if (authority == null || clientId == null || clientSecret == null)
                throw new BusinessException("Auth:ConfigurationMissing");

            var tokenEndpoint = $"{authority}/connect/token";

            var payload = new Dictionary<string, string>
            {
                ["grant_type"] = "refresh_token",
                ["refresh_token"] = refreshToken,
                ["client_id"] = clientId,
                ["client_secret"] = clientSecret
            };

            using var req = new HttpRequestMessage(HttpMethod.Post, tokenEndpoint)
            { Content = new FormUrlEncodedContent(payload) };

            using var res = await client.SendAsync(req);

            if (!res.IsSuccessStatusCode)
            {
                var body = await res.Content.ReadAsStringAsync();
                throw new BusinessException("Auth:TokenEndpointFailed")
                    .WithData("Status", (int)res.StatusCode)
                    .WithData("Body", body);
            }

            var dto = await res.Content.ReadFromJsonAsync<TokenEndpointResponse>();
            if (dto is null)
                throw new BusinessException("Auth:TokenParseFailed");

            return new TokenResponseDto
            {
                AccessToken = dto.access_token,
                RefreshToken = dto.refresh_token,
                ExpiresIn = dto.expires_in
            };
        }

        public async Task SendResetCodeAsync(string email)
        {
            var u = await _userManager.FindByEmailAsync(email);
            if (u == null) return;

            var token = await _userManager.GeneratePasswordResetTokenAsync(u);
            var encoded = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            var frontendUrl = _configuration["App:FrontendUrl"];

            string? link = null;
            if (!string.IsNullOrWhiteSpace(frontendUrl))
            {
                link = $"{frontendUrl}/reset-password?userId={u.Id}&token={encoded}";
            }

            string message = string.Empty;
            if (link != null)
            {
                message = $"Click to reset: <a href=\"{link}\">Reset Password</a>";
            }
            else
            {
                message = $"Use this token:\n\nUserId: {u.Id}\nToken: {encoded}";
            }

            try
            {
                await _emailSender.SendAsync(email, "Reset your password", message, true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Send reset email failed (email={Email})", email);
            }
        }

        public async Task ResetAsync(string userId, string resetToken, string newPassword)
        {
            var u = await _userManager.GetByIdAsync(Guid.Parse(userId));
            var token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(resetToken));

            (await _userManager.ResetPasswordAsync(u, token, newPassword)).CheckErrors();
        }

        public async Task ChangeAsync(string currentPassword, string newPassword)
        {
            var userId = _currentUser.Id;
            if (userId == null)
                throw new AbpAuthorizationException("Bạn chưa đăng nhập.");

            if (string.IsNullOrWhiteSpace(currentPassword) || string.IsNullOrWhiteSpace(newPassword))
                throw new UserFriendlyException("Thiếu mật khẩu hiện tại hoặc mật khẩu mới.");

            if (currentPassword == newPassword)
                throw new UserFriendlyException("Mật khẩu mới phải khác mật khẩu hiện tại.");

            var u = await _userManager.GetByIdAsync(userId.Value);

            if (!await _userManager.HasPasswordAsync(u))
                throw new UserFriendlyException("Tài khoản chưa có mật khẩu. Hãy đặt mật khẩu lần đầu (AddPassword) thay vì ChangePassword.");

            var result = await _userManager.ChangePasswordAsync(u, currentPassword, newPassword);
            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => $"{e.Code}: {e.Description}"));
                throw new UserFriendlyException("Đổi mật khẩu thất bại: " + errors);
            }
        }
    }
}
