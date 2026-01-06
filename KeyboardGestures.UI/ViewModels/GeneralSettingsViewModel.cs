using KeyboardGestures.Core.Settings;
using ReactiveUI;

namespace KeyboardGestures.UI.ViewModels
{
    public class GeneralSettingsViewModel : ReactiveObject
    {

        private bool _launchOnStartup;
        public bool LaunchOnStartup
        {
            get => _launchOnStartup;
            set => this.RaiseAndSetIfChanged(ref _launchOnStartup, value);
        }

        public GeneralSettingsViewModel(AppSettings settings)
        {

        }
    }
}
