using Microsoft.EntityFrameworkCore;
using NextHireApp.EntityFrameworkCore;
using NextHireApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NextHireApp.Repository
{
    public class UserCvRepository : IUserCvRepository
    {
        private readonly INextHireAppDbContext _context;
        private readonly INextHireAppReadDbContext _readContext;

        public UserCvRepository(INextHireAppDbContext context, INextHireAppReadDbContext readContext)
        {
            this._context = context;
            this._readContext = readContext;
        }

        public async Task<UserCV> CreateAsync(UserCV userCv)
        {
            try
            {
                await _context.UserCVs.AddAsync(userCv);
                await _context.SaveChangesAsync();
                return userCv;
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }

        }

        public async Task<bool> DeleteAsync(Guid cvId)
        {
            var cv = await _context.UserCVs.FindAsync(cvId);
            if (cv == null)
            {
                return false;
            }

            _context.UserCVs.Remove(cv);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(Guid cvId)
        {
            return await _readContext.UserCVs.AsNoTracking().AnyAsync(x => x.CvId == cvId);
        }

        public async Task<UserCV?> GetByIdAsync(Guid cvId,bool isTracked = false)
        {
            var dbSet = isTracked ? _readContext.UserCVs : _readContext.UserCVs.AsNoTracking();

            return await dbSet
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.CvId == cvId);
        }

        public async Task<List<UserCV>> GetByUserCodeAsync(string userCode, bool isTracked = false)
        {
            var dbSet = isTracked ? _readContext.UserCVs : _readContext.UserCVs.AsNoTracking();

            return await dbSet
                .Where(x => x.UserCode == userCode)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task<UserCV?> GetDefaultCvByUserCodeAsync(string userCode, bool isTracked = false)
        {
            var dbSet = isTracked ? _readContext.UserCVs : _readContext.UserCVs.AsNoTracking();

            return await dbSet
                .FirstOrDefaultAsync(x => x.UserCode == userCode && x.IsDefault);
        }

        public async Task<bool> SetAsDefaultAsync(Guid cvId, string userCode)
        {
            var cv = await _readContext.UserCVs
                .FirstOrDefaultAsync(x => x.CvId == cvId && x.UserCode == userCode);
            
            if (cv == null)
            {
                return false;
            }

            cv.IsDefault = true;
            await _readContext.SaveChangesAsync();
            return true;
        }

        public async Task<UserCV> UpdateAsync(UserCV userCv)
        {
            _context.UserCVs.Update(userCv);
            await _context.SaveChangesAsync();
            return userCv;
        }

        public async Task<bool> UnsetDefaultsForUserAsync(string userCode)
        {
            var defaultCvs = await _context.UserCVs
                .Where(x => x.UserCode == userCode && x.IsDefault)
                .ToListAsync();

            if (!defaultCvs.Any())
            {
                return true;
            }

            foreach (var cv in defaultCvs)
            {
                cv.IsDefault = false;
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }
}