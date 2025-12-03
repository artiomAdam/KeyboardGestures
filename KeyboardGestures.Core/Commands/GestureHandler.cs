using KeyboardGestures.Core.Gestures;

namespace KeyboardGestures.Core.Commands
{
    public class GestureHandler
    {

        public GestureHandler(IGestureInterpreter interpreter, CommandRegistry registry, ICommandExecutor executor) 
        {
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
