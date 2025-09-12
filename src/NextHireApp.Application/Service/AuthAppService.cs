using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using NextHireApp.Dtos;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Identity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Users;
using IdentityUser = Volo.Abp.Identity.IdentityUser;


namespace NextHireApp.Service
{
    public class AuthAppService : ApplicationService, IAuthAppService
    {
        private readonly IdentityUserManager _userManager;
        private readonly IHttpClientFactory _httpClientFactory; // inject để call /connect/token
        private readonly IConfiguration _configuration;
        private readonly IDistributedCache _cache;
        private readonly ITokenBlacklistService _blacklist;

        public AuthAppService(
            IdentityUserManager userManager,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            IDistributedCache cache,
            ITokenBlacklistService blacklist)
        {
            _userManager = userManager;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _cache = cache;
            _blacklist = blacklist;
        }

        public async Task RegisterAsync(RegisterDto input)
        {
            var user = new IdentityUser(
                GuidGenerator.Create(), input.UserName, input.Email, CurrentTenant.Id);

            (await _userManager.CreateAsync(user, input.Password)).CheckErrors();
        }

        public async Task<TokenResponseDto> PasswordLoginAsync(LoginDto input)
        {
            var user = await FindByNameOrEmailAsync(input.UserNameOrEmail);
            if (user == null || !await _userManager.CheckPasswordAsync(user, input.Password))
                throw new BusinessException("Auth:InvalidCredentials");

            // Gọi OAuth2 Password flow tới /connect/token
            var client = _httpClientFactory.CreateClient();
            var authority = _configuration["AuthServer:Authority"];
            var clientId = _configuration["AuthServer:SwaggerClientId"];
            var clientSecret = _configuration["AuthServer:SwaggerClientSecret"];
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

        public async Task RequestPasswordResetAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email)
                       ?? throw new BusinessException("Auth:UserNotFound");
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            // gửi email
        }

        public async Task ChangePasswordAsync(string current, string @new)
        {
            var user = await _userManager.GetByIdAsync(CurrentUser.GetId());
            (await _userManager.ChangePasswordAsync(user, current, @new)).CheckErrors();
        }

        private async Task<IdentityUser> FindByNameOrEmailAsync(string usernameOrEmail)
        {
            return await _userManager.FindByNameAsync(usernameOrEmail)
                   ?? await _userManager.FindByEmailAsync(usernameOrEmail)
                   ?? throw new BusinessException("Auth:UserNotFound");
        }

        private sealed record TokenEndpointResponse(
            string access_token, string token_type, long expires_in, string refresh_token);
    }
}
