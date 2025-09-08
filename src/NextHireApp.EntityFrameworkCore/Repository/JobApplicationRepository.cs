using Microsoft.EntityFrameworkCore;
using NextHireApp.EntityFrameworkCore;
using NextHireApp.JobApplications;
using NextHireApp.Model;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;

namespace NextHireApp.Repository
{
    public class JobApplicationRepository : IJobApplicationRepository
    {
        private readonly INextHireAppDbContext _context;
        private readonly INextHireAppReadDbContext _readContext;
        public JobApplicationRepository(INextHireAppDbContext context, INextHireAppReadDbContext readContext)
        {
            _context = context;
            _readContext = readContext;
        }

        public async Task<JobApplication?> GetByCodeAsync(string applicationCode, bool isTracked = false)
        {
            var dbSet = isTracked ?  _readContext.JobApplications : _context.JobApplications.AsNoTracking();
            return await dbSet.Where(c => c.ApplicationCode == applicationCode).FirstOrDefaultAsync();
        }

        public async Task<List<JobApplication>> GetByJobCodeAsync(string jobCode, bool isTracked = false)
        {
            var dbSet = isTracked ? _readContext.JobApplications : _context.JobApplications.AsNoTracking();
            return await dbSet.Where(c => c.JobCode == jobCode).ToListAsync();
        }

        public async Task<List<JobApplication>> GetByUserCodeAsync(string userCode, bool isTracked = false)
        {
            var dbSet = isTracked ? _readContext.JobApplications : _context.JobApplications.AsNoTracking();
            return await dbSet.Where(c => c.UserCode == c.UserCode ).AsNoTracking().ToListAsync();
        }

        public Task<bool> HasUserAppliedToJobAsync(string userCode, string jobCode)
        {
           return _readContext.JobApplications.AsNoTracking().AnyAsync(c=> c.UserCode == userCode && c.JobCode == c.JobCode && c.Status != (int)ApplicationStatus.Cancelled);
        }

        public async Task<JobApplication> InsertAsync(JobApplication entity)
        {
            try
            {
                var entry = await _context.Set<JobApplication>().AddAsync(entity);
                await _context.SaveChangesAsync();
                return entry.Entity;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<JobApplication> UpdateAsync(JobApplication entity)
        {
            try
            {
                var existingEntity = await _context.JobApplications
                .Where(x => x.ApplicationCode == entity.ApplicationCode)
                .FirstOrDefaultAsync();

                if (existingEntity != null)
                {
                    existingEntity.Status = entity.Status;
                    existingEntity.JobApplicateVersion += 1;

                    await _context.SaveChangesAsync();
                    return existingEntity;
                }
                return entity;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
