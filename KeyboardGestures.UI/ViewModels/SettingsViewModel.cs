using ReactiveUI;

namespace KeyboardGestures.UI.ViewModels
{
    public class SettingsViewModel : ReactiveObject
    {
        public object CommandSettings { get; set; }
        public object GeneralSettings { get; set; }

        private object _selectedTab;
        public object SelectedTab
        {
            get => _selectedTab;
            set => this.RaiseAndSetIfChanged(ref _selectedTab, value);
        }

        public SettingsViewModel(CommandSettingsViewModel commandSettingsVM, GeneralSettingsViewModel generalSettingsVM)
        {
            CommandSettings = commandSettingsVM;
            GeneralSettings = generalSettingsVM;
            
            SelectedTab = GeneralSettings;
        }
    }
}
