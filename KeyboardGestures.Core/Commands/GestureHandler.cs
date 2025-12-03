using KeyboardGestures.Core.Gestures;

namespace KeyboardGestures.Core.Commands
{
    public class GestureHandler
    {
        public event Action? OverlayShouldOpen;
        public event Action? OverlayShouldClose;

        public GestureHandler(IGestureInterpreter interpreter, CommandRegistry registry, ICommandExecutor executor) 
        {
            interpreter.OverlayActivated += () =>
            {
                OverlayShouldOpen?.Invoke();
            };

            interpreter.OverlayDeactivated += () =>
            {
                OverlayShouldClose?.Invoke();
            };

            interpreter.SequenceCompleted += seq =>
            {
                var cmd = registry.FindBySequence(seq);
                if (cmd != null)
                {
                    executor.Execute(cmd);
                }
            };

        }
    }
}
