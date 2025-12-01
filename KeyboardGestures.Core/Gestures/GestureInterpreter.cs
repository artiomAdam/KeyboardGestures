using KeyboardGestures.Core.Events;
using System.Diagnostics;


namespace KeyboardGestures.Core.Gestures
{
    public class GestureInterpreter : IGestureInterpreter
    {
        private readonly List<int> _buffer = new();

        public void OnKeyEvent(KeyEvent ev)
        {
            if (ev.Type != KeyEventType.KeyDown)
                return;

            _buffer.Add(ev.VirtualKeyCode);

            // for now just print — later will detect sequences
            Debug.WriteLine($"Key: {ev.VirtualKeyCode}");
        }
    }
}
