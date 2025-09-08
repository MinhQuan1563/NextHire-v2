using NextHireApp.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace NextHireApp.Repository
{
    public interface IJobApplicationRepository : ITransientDependency
    {
        Task<JobApplication> InsertAsync(JobApplication entity);
        Task<JobApplication> UpdateAsync(JobApplication entity);
        Task<JobApplication?> GetByCodeAsync(string applicationCode,bool isTracked = false);
        Task<List<JobApplication>> GetByUserCodeAsync(string userCode, bool isTracked = false);
        Task<List<JobApplication>> GetByJobCodeAsync(string jobCode, bool isTracked = false);
        Task<bool> HasUserAppliedToJobAsync(string userCode, string jobCode);
    }
}
