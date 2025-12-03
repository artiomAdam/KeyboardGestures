using KeyboardGestures.Core.Events;

namespace KeyboardGestures.Core.Gestures
{
    public interface IGestureInterpreter
    {
        void OnKeyEvent(KeyEvent ev);
        public event Action<List<int>>? SequenceCompleted;
        public event Action? OverlayActivated;
        public event Action? OverlayDeactivated;
    }
}
