using KeyboardGestures.Core.Gestures;

namespace KeyboardGestures.Core.Settings
{
    public class AppSettings
    {
        private int _activationKey = 0x11;

        public int ActivationKey
        {
            get => _activationKey;
            set => _activationKey = GestureInterpreter.NormalizeKey(value);
        } // TODO: add a reset to default
        public bool LaunchOnStartup { get; set; } = false;
    }
}
   