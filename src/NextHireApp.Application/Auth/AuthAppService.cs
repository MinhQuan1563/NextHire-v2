using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using NextHireApp.Auth.Dtos;
using NextHireApp.Auth.Interfaces;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Caching;
using Volo.Abp.Identity;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace NextHireApp.Auth
{
    public class AuthAppService : ApplicationService, IAuthAppService
    {
        private readonly IdentityUserManager _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ITokenService _tokenService;

        public AuthAppService(
            IdentityUserManager userManager,
            SignInManager<IdentityUser> signInManager,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        public async Task RegisterAsync(RegisterDto input)
        {
            var user = new IdentityUser(GuidGenerator.Create(), input.UserName, input.Email);
            (await _userManager.CreateAsync(user, input.Password)).CheckErrors();
        }

        public async Task<TokenResponseDto> LoginAsync(LoginDto input)
        {
            IdentityUser? user =
                input.UserNameOrEmail.Contains("@")
                ? await _userManager.FindByEmailAsync(input.UserNameOrEmail)
                : await _userManager.FindByNameAsync(input.UserNameOrEmail);

            if (user is null) 
                throw new UserFriendlyException("User not found");

            var result = await _signInManager.CheckPasswordSignInAsync(user, input.Password, true);
            if (!result.Succeeded) throw new UserFriendlyException("Invalid credentials");

            return await _tokenService.IssueAsync(user);
        }

        public async Task<TokenResponseDto> RefreshAsync(RefreshRequestDto input)
        {
            var cache = LazyServiceProvider.LazyGetRequiredService<IDistributedCache>();
            var userIdStr = await cache.GetStringAsync("jwt:refresh:" + input.RefreshToken);
            if (string.IsNullOrEmpty(userIdStr)) 
                throw new UserFriendlyException("Invalid refresh");

            var user = await _userManager.GetByIdAsync(Guid.Parse(userIdStr));
            return await _tokenService.IssueAsync(user, input.RefreshToken);
        }

        public Task LogoutAsync(string accessTokenJti, DateTime accessExpiryUtc)
        {
            throw new NotImplementedException();
        }
    }
}
