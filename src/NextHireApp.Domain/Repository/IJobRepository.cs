using NextHireApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace NextHireApp.Repository
{
    public interface IJobRepository : ITransientDependency
    {
        Task<bool> IsExists(string jobCode);
        Task<Job?> GetJobByCodeAsync(string jobCode, bool isTracked = false);
    }
}
