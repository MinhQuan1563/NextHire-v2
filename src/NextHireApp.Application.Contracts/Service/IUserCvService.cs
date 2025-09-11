using NextHireApp.Dtos;
using NextHireApp.UserCVs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NextHireApp.Service
{
    public interface IUserCvService
    {
        Task<UserCvDto> CreateUserCvAsync(CreateUserCvDto input);
        Task<UserCvDetailDto> GetUserCvAsync(Guid cvId);
        Task<List<UserCvDto>> GetUserCvsByUserCodeAsync(string userCode);
        Task<UserCvDetailDto> GetDefaultUserCvAsync(string userCode);
        Task<UserCvDto> SetDefaultUserCvAsync(SetDefaultUserCvDto input);
        Task<bool> DeleteUserCvAsync(Guid cvId, string userCode);
        Task<UserCvDto> UpdateUserCvAsync(UpdateUserCvDto input);
    }
}