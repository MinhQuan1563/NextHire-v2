using NextHireApp.Dtos;
using NextHireApp.Model;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Identity;
using Volo.Abp.Users;

namespace NextHireApp.Service
{
    public class UserProfileService : ApplicationService, IUserProfileService
    {
        private readonly IdentityUserManager _userManager;

        public UserProfileService(IdentityUserManager userManager)
        {
            _userManager = userManager;
        }

        public async Task<UserProfileDto> GetAsync()
        {
            //var u = await _userManager.GetByIdAsync(CurrentUser.GetId());
            //return new UserProfileDto
            //{
            //    Id = u.Id,
            //    FullName = u.Name,
            //    Email = u.Email,
            //    PhoneNumber = u.PhoneNumber,
            //    AvatarUrl = (u as AppUser)?.AvatarUrl
            //};

            return new UserProfileDto();
        }

        public async Task<UserProfileDto> UpdateAsync(UserProfileDto input)
        {
            //var u = (await _userManager.GetByIdAsync(CurrentUser.GetId())) as AppUser;
            //u!.Name = input.FullName;
            //u.PhoneNumber = input.PhoneNumber;
            //u.AvatarUrl = input.AvatarUrl;
            //(await _userManager.UpdateAsync(u)).CheckErrors();
            //return await GetAsync();
            return input;
        }
    }
}
