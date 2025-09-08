using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NextHireApp.Dtos;
using NextHireApp.JobApplications;
using NextHireApp.Model;

namespace NextHireApp.Service
{
    public interface IJobApplicationService 
    {
        Task<JobApplicationDto> CreateJobApplicationAsync(CreateJobApplicationDto input);
        Task<List<JobApplicationDto>> GetJobApplicationsByUserCodeAsync(string userCode);
        Task<List<JobApplicationDto>> GetJobApplicationsByJobCodeAsync(string jobCode);
        Task<JobApplicationDto> UpdateApplicationStatusAsync(UpdateJobApplicationStatusDto input);
        Task<bool> HasUserAppliedToJobAsync(string userCode, string jobCode);
        Task<bool> CancelJobApplicationAsync(string applicationCode, string userCode);
        Task<JobApplicationDetailDto> GetJobApplicationDetailByCodeAsync(string applicationCode);
    }
}
