using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NextHireApp.Dtos;
using NextHireApp.Service;
using System.Threading.Tasks;

namespace NextHireApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class UserProfileController
    {
        private readonly IUserProfileService _userProfileService;
        public UserProfileController(IUserProfileService userProfileService)
        {
            _userProfileService = userProfileService;
        }

        [HttpGet]
        public async Task<UserProfileDto> Get()
        {
            return await _userProfileService.GetAsync();
        }

        [HttpPut] 
        public async Task<UserProfileDto> Update(UserProfileDto input)
        {
            return await _userProfileService.UpdateAsync(input);
        }
    }
}
