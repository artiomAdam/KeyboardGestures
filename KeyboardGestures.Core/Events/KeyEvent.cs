namespace KeyboardGestures.Core.Events
{
    public enum KeyEventType
    {
        KeyDown,
        KeyUp
    }

    public record KeyEvent(KeyEventType Type, int VirtualKeyCode);
}
