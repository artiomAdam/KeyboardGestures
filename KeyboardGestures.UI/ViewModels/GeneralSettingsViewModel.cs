using KeyboardGestures.Core.Commands;
using KeyboardGestures.Core.Events;
using KeyboardGestures.Core.KeyboardHook;
using KeyboardGestures.Core.Settings;
using KeyboardGestures.UI.Services;
using ReactiveUI;
using System.Reactive;

namespace KeyboardGestures.UI.ViewModels
{
    public class GeneralSettingsViewModel : ReactiveObject
    {

        private readonly IAppSettingsService _settings;
        private readonly IKeyboardHookService _keyboard;
        private bool _isRecording;

        private int _activationKey;
        public int ActivationKey
        {
            get => _activationKey;
            private set
            {
                if (_activationKey != value)
                {
                    _activationKey = value;
                    this.RaisePropertyChanged();
                    this.RaisePropertyChanged(nameof(ActivationKeyDisplay));
                }
            }
        }
        public string ActivationKeyDisplay => _settings.Current.ActivationKey == 0 ? "—" : CommandDisplayHelper.ToDisplayName(ActivationKey);


        // TODO: unimplemented placeholder feature
        public bool LaunchOnStartup
        {
            get => _settings.Current.LaunchOnStartup;
            set
            {
                if (_settings.Current.LaunchOnStartup != value)
                {
                    _settings.Current.LaunchOnStartup = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        public ReactiveCommand<Unit, Unit> ApplyCommand { get; }
        public ReactiveCommand<Unit, Unit> CancelCommand { get; }

        public GeneralSettingsViewModel(AppSettingsService settingsService, IKeyboardHookService keyboardHookService)
        {
            _settings = settingsService;
            _keyboard = keyboardHookService;
             
            ActivationKey = _settings.Current.ActivationKey;

            ApplyCommand = ReactiveCommand.Create(() =>
            {
                if (_settings.Current.ActivationKey != ActivationKey)
                {
                    _settings.Current.ActivationKey = ActivationKey;
                    _settings.Save();
                }
            });

            CancelCommand = ReactiveCommand.Create(() =>
            {
                // close?

                // restore to prev?
                ActivationKey = _settings.Current.ActivationKey;
            });
        }

        private void OnKey(KeyEvent ev)
        {
            if (!_isRecording) return;
            if (ev.Type != KeyEventType.KeyDown) return;
              
            int vk = ev.VirtualKeyCode;

            ActivationKey = vk;
            StopRecording();
        }

        public void BeginRecording()
        {
            if (_isRecording) return;
            _keyboard.KeyEventReceived += OnKey;
            _isRecording = true;
        }

        public void StopRecording()
        {
            _keyboard.KeyEventReceived -= OnKey;
            _isRecording = false;
        }
    }
}
