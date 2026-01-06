using KeyboardGestures.Core.Settings;
using ReactiveUI;

namespace KeyboardGestures.UI.ViewModels
{
    public class GeneralSettingsViewModel : ReactiveObject
    {

        private readonly IAppSettingsService _settings;
        public int ActivationKey
        {
            get => _settings.Current.ActivationKey;
            set
            {
                if (_settings.Current.ActivationKey == value) return;
                _settings.Current.ActivationKey = value;
                this.RaisePropertyChanged();
            }
        }


        public GeneralSettingsViewModel(IAppSettingsService settingsService)
        {
            _settings = settingsService;
        }
    }
}
