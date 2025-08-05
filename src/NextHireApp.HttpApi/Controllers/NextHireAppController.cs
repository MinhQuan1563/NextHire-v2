using NextHireApp.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace NextHireApp.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class NextHireAppController : AbpControllerBase
{
    protected NextHireAppController()
    {
        LocalizationResource = typeof(NextHireAppResource);
    }
}
