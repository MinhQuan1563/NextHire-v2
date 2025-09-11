using NextHireApp.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace NextHireApp.Repository
{
    public interface IUserCvRepository : ITransientDependency
    {
        Task<UserCV?> GetByIdAsync(Guid cvId, bool isTracked = false);
        Task<List<UserCV>> GetByUserCodeAsync(string userCode,bool isTracked = false);
        Task<UserCV?> GetDefaultCvByUserCodeAsync(string userCode, bool isTracked = false);
        Task<bool> SetAsDefaultAsync(Guid cvId, string userCode);
        Task<bool> UnsetDefaultsForUserAsync(string userCode);
        Task<UserCV> CreateAsync(UserCV userCv);
        Task<UserCV> UpdateAsync(UserCV userCv);
        Task<bool> DeleteAsync(Guid cvId);
        Task<bool> ExistsAsync(Guid cvId);
    }
}