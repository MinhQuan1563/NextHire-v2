using Microsoft.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Threading;

namespace NextHireApp.EntityFrameworkCore;

public static class NextHireAppEfCoreEntityExtensionMappings
{
    private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

    public static void Configure()
    {
        NextHireAppGlobalFeatureConfigurator.Configure();
        NextHireAppModuleExtensionConfigurator.Configure();

        OneTimeRunner.Run(() =>
        {

        });
    }

}
