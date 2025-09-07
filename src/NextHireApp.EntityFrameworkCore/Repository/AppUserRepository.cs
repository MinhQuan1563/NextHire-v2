using Microsoft.EntityFrameworkCore;
using NextHireApp.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextHireApp.Repository
{
    public class AppUserRepository : IAppUserRepository
    {
        private readonly NextHireAppDbContext _dbContext;

        public AppUserRepository(NextHireAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> IsExists(string userCode)
        {
            return await _dbContext.AppUsers.AnyAsync(u => u.UserCode == userCode);
        }
    }
}
