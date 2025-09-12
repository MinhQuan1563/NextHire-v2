using AbpIdentityUser = Volo.Abp.Identity.IdentityUser;
using AbpIdentityRole = Volo.Abp.Identity.IdentityRole;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Security.Claims;
using NextHireApp.EntityFrameworkCore;
using NextHireApp.Model;

namespace NextHireApp
{
    // Thay thế factory mặc định của ABP
    [Dependency(ReplaceServices = true)]
    [ExposeServices(typeof(IUserClaimsPrincipalFactory<AbpIdentityUser>))]
    public class UserClaimsPrincipalFactory : AbpUserClaimsPrincipalFactory, IScopedDependency
    {
        private readonly IDbContextProvider<NextHireAppDbContext> _dbContextProvider;

        public UserClaimsPrincipalFactory(
            UserManager<AbpIdentityUser> userManager,
            RoleManager<AbpIdentityRole> roleManager,
            IOptions<IdentityOptions> optionsAccessor,
            IDbContextProvider<NextHireAppDbContext> dbContextProvider,
            ICurrentPrincipalAccessor currentPrincipalAccessor,
            IAbpClaimsPrincipalFactory abpClaimsPrincipalFactory)
            : base(userManager, roleManager, optionsAccessor,
                   currentPrincipalAccessor, abpClaimsPrincipalFactory)
        {
            _dbContextProvider = dbContextProvider;
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(AbpIdentityUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);

            var db = await _dbContextProvider.GetDbContextAsync();
            var appUser = await db.Set<AppUser>()
                                  .FirstOrDefaultAsync(x => x.Id == user.Id);

            if (appUser != null)
            {
                identity.AddClaim(new Claim("user_code", appUser.UserCode));
            }

            return identity;
        }
    }
}
