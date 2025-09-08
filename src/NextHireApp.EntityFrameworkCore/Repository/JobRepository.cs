using Microsoft.EntityFrameworkCore;
using NextHireApp.EntityFrameworkCore;
using NextHireApp.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace NextHireApp.Repository
{
    public class JobRepository : IJobRepository
    {
        private readonly INextHireAppDbContext _context;
        private readonly INextHireAppReadDbContext _readContext;
        public JobRepository(INextHireAppDbContext context, INextHireAppReadDbContext readContext)
        {
            _context = context;
            _readContext = readContext;
        }

        public async Task<Job?> GetJobByCodeAsync(string jobCode,bool isTracked = false)
        {
            var dbSet = isTracked ? _readContext.Jobs : _readContext.Jobs.AsNoTracking();
            return await dbSet.FirstOrDefaultAsync(x => x.JobCode == jobCode);
        }

        public Task<bool> IsExists(string jobCode)
        {
            return _readContext.Jobs.AnyAsync(x=>x.JobCode == jobCode);
        }
    }
}
