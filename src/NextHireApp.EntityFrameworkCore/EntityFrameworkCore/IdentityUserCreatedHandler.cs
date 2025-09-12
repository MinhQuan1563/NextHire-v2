using Microsoft.EntityFrameworkCore;
using NextHireApp.EntityFrameworkCore;
using NextHireApp.Model;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Uow;
using Volo.Abp.Users;

namespace NextHireApp
{

    public class IdentityUserCreatedHandler
        : IDistributedEventHandler<EntityCreatedEto<UserEto>>, ITransientDependency
    {
        private readonly IDbContextProvider<NextHireAppDbContext> _dbCtxProvider;

        public IdentityUserCreatedHandler(
            IDbContextProvider<NextHireAppDbContext> dbCtxProvider)
        {
            _dbCtxProvider = dbCtxProvider;
        }

        [UnitOfWork]
        public async Task HandleEventAsync(EntityCreatedEto<UserEto> eventData)
        {
            var u = eventData.Entity;
            var db = await _dbCtxProvider.GetDbContextAsync();
            var set = db.Set<AppUser>();

            // Lấy user có UserCode lớn nhất
            var lastUser = await set.OrderByDescending(x => x.UserCode)
                                    .FirstOrDefaultAsync();

            int nextNumber = 1000;
            if (lastUser != null &&
                int.TryParse(lastUser.UserCode.Replace("USR", ""), out var lastNumber))
            {
                nextNumber = lastNumber + 1;
            }

            var existed = await set.AnyAsync(x => x.Id == u.Id);
            if (!existed)
            {
                var appUser = new AppUser
                {
                    Id = u.Id,
                    UserCode = $"USR{nextNumber:D4}",
                    FullName = string.IsNullOrWhiteSpace(u.Name) ? u.UserName : $"{u.Name} {u.Surname}".Trim()
                };

                await set.AddAsync(appUser);
                await db.SaveChangesAsync();
            }
        }
    }
}
