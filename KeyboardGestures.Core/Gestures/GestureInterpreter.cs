using KeyboardGestures.Core.Events;
using System.Diagnostics;


namespace KeyboardGestures.Core.Gestures
{
    public class GestureInterpreter : IGestureInterpreter
    {
        private const int VK_CONTROL = 0xA2;

        private bool _ctrlDown = false;
        private readonly List<int> _sequence = new();

        public event Action<List<int>>? SequenceCompleted;

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
            if(vk == VK_CONTROL)
            {
                if(!_ctrlDown)
                {
                    _ctrlDown = true;
                    _sequence.Clear();
                }
                return;
            }

            if (!_ctrlDown) return;

            _sequence.Add(vk);
            Debug.WriteLine("Sequence: " + string.Join(", ", _sequence));
        }

        private void HandleKeyUp(int vk)
        {
            if(vk == VK_CONTROL)
            {
                if(_ctrlDown)
                {
                    _ctrlDown = false;
                    FinalizeSequence();
                }
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
    }
}
