using KeyboardGestures.Core.Events;
using System.Diagnostics;


namespace KeyboardGestures.Core.Gestures
{
    public class GestureInterpreter : IGestureInterpreter
    {
        private const int VK_CONTROL = 0x11;
        private const int VK_SPACE = 0x20;

        private bool _ctrlDown = false;
        private readonly List<int> _sequence = new();
        public event Action? OverlayActivated;
        public event Action? OverlayDeactivated;

        public event Action<List<int>>? SequenceCompleted;


        private readonly HashSet<int> _pressedKeys = new();
        private bool _overlayActive = false;


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
            vk = NormalizeCtrl(vk);
            if (_pressedKeys.Contains(vk)) return;
            _pressedKeys.Add(vk);
            if (vk == VK_CONTROL)
            {
                if(!_ctrlDown)
                {
                    _ctrlDown = true;
                    _sequence.Clear();
                }
                return;
            }

            if (_ctrlDown && vk == VK_SPACE)
            {
                OverlayActivated?.Invoke();
                _overlayActive = true;
                return;
            }

            if (!_ctrlDown) return;

            _sequence.Add(vk);
            Debug.WriteLine("Sequence: " + string.Join(", ", _sequence));
        }

        private void HandleKeyUp(int vk)
        {
            vk = NormalizeCtrl(vk);
            _pressedKeys.Remove(vk);
            if(vk == VK_CONTROL)
            {
                if(_ctrlDown)
                {
                    _ctrlDown = false;
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
            Debug.WriteLine($"Sequence COMPLETE: [{string.Join(", ", seq)}]");

            SequenceCompleted?.Invoke( seq );
        }

        private static int NormalizeCtrl(int vk)
        {
            return (vk == 0xA2 || vk == 0xA3) ? 0x11 : vk;
        }
    }
}
