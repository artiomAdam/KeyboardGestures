using KeyboardGestures.Core.Events;
using KeyboardGestures.Core.JsonStorage;
using KeyboardGestures.Core.Settings;
using System.Diagnostics;


namespace KeyboardGestures.Core.Gestures
{
    public class GestureInterpreter : IGestureInterpreter
    {
        //private const int VK_CONTROL = 0x11;
        private const int VK_SPACE = 0x20;

        private bool _activationKeyDown = false;
        private readonly List<int> _sequence = new();
        public event Action? OverlayActivated;
        public event Action? OverlayDeactivated;

        public event Action<List<int>>? SequenceCompleted;


        private readonly HashSet<int> _pressedKeys = new();
        private bool _overlayActive = false;

        private readonly AppSettings _settings;

        public GestureInterpreter(AppSettings settings)
        {
            _settings = settings;
        }

        public void OnKeyEvent(KeyEvent ev)
        {
            if (ev.Type == KeyEventType.KeyDown)
            {
                HandleKeyDown(ev.VirtualKeyCode);
            }
            else if (ev.Type == KeyEventType.KeyUp)
            {
                HandleKeyUp(ev.VirtualKeyCode);
            }
        }

        private void HandleKeyDown(int vk)
        {
            vk = NormalizeKey(vk);
            if (_pressedKeys.Contains(vk)) return;
            _pressedKeys.Add(vk);
            if (vk == _settings.ActivationKey)
            {
                if(!_activationKeyDown)
                {
                    _activationKeyDown = true;
                    _sequence.Clear();
                }
                return;
            }

            if (_activationKeyDown && vk == VK_SPACE)
            {
                OverlayActivated?.Invoke();
                _overlayActive = true;
                return;
            }

            if (!_activationKeyDown) return;

            _sequence.Add(vk);
           // Debug.WriteLine("Sequence: " + string.Join(", ", _sequence));
        }

        private void HandleKeyUp(int vk)
        {
            vk = NormalizeKey(vk);
            _pressedKeys.Remove(vk);
            if(vk == _settings.ActivationKey)
            {
                if(_activationKeyDown)
                {
                    _activationKeyDown = false;
                    if (_overlayActive)
                    {
                        _overlayActive = false;
                        OverlayDeactivated?.Invoke();
                        return;
                    }
                    FinalizeSequence();
                }
            }
            else
            {
                // _sequence.Remove(vk);
            }
        }

        private void FinalizeSequence()
        {
            if (_sequence.Count == 0) return;
            var seq = _sequence.ToList(); // clone
            _sequence.Clear();
            //Debug.WriteLine($"Sequence COMPLETE: [{string.Join(", ", seq)}]");

            SequenceCompleted?.Invoke( seq );
        }

        public static int NormalizeKey(int vk)
        {
            return vk switch
            {
                0xA2 or 0xA3 => 0x11, // ctrl
                0xA0 or 0xA1 => 0x10, //shift
                0xA4 or 0xA5 => 0x12, //alt
                _ => vk
            }; // TODO: see if other dupes need to be added
        }
    }
}
