using System;
using System.Collections.Generic;
using System.Text;
using NextHireApp.Localization;
using Volo.Abp.Application.Services;

namespace NextHireApp;

/* Inherit your application services from this class.
 */
public abstract class NextHireAppAppService : ApplicationService
{
    protected NextHireAppAppService()
    {
        LocalizationResource = typeof(NextHireAppResource);
    }
}
