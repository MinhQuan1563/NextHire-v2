using Microsoft.Extensions.Localization;
using NextHireApp.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace NextHireApp;

[Dependency(ReplaceServices = true)]
public class NextHireAppBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<NextHireAppResource> _localizer;

    public NextHireAppBrandingProvider(IStringLocalizer<NextHireAppResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
