using NextHireApp.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextHireApp.Service
{
    public interface IUserProfileService
    {
        Task<UserProfileDto> GetAsync();
        Task<UserProfileDto> UpdateAsync(UserProfileDto input);
    }
}
