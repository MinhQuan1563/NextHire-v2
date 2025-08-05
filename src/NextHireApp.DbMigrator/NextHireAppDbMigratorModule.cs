using NextHireApp.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace NextHireApp.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(NextHireAppEntityFrameworkCoreModule),
    typeof(NextHireAppApplicationContractsModule)
    )]
public class NextHireAppDbMigratorModule : AbpModule
{
}
