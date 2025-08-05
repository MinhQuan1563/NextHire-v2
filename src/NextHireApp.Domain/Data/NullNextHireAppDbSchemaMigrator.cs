using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace NextHireApp.Data;

/* This is used if database provider does't define
 * INextHireAppDbSchemaMigrator implementation.
 */
public class NullNextHireAppDbSchemaMigrator : INextHireAppDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
