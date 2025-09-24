using DataInterfaceConsole.Actions.Settings;

namespace DataInterfaceConsole.Actions.EphermalSettings;

internal class ManageEphermalSettings : BaseManageSettingsMenu {
    public override string Name => "Ephermal Settings and Triggers";

    public override ISettingsContainer SettingsHandler => Program.instance.eh;    
}
