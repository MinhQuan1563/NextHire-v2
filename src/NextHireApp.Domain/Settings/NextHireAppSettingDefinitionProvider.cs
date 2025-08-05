using Volo.Abp.Settings;

namespace NextHireApp.Settings;

public class NextHireAppSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(NextHireAppSettings.MySetting1));
    }
}
