using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NextHireApp.Data;
using Volo.Abp.DependencyInjection;

namespace NextHireApp.EntityFrameworkCore;

public class EntityFrameworkCoreNextHireAppDbSchemaMigrator
    : INextHireAppDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreNextHireAppDbSchemaMigrator(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolve the NextHireAppDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<NextHireAppDbContext>()
            .Database
            .MigrateAsync();
    }
}
