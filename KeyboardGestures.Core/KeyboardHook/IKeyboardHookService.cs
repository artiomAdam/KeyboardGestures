using KeyboardGestures.Core.Events;

namespace KeyboardGestures.Core.KeyboardHook
{
    public interface IKeyboardHookService : IDisposable
    {
        event Action<KeyEvent>? KeyEventReceived;
        void Start();
        void Stop();
    }
}
