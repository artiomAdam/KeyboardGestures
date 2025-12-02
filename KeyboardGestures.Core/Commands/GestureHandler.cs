using KeyboardGestures.Core.Gestures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    executor.Execute(cmd.ActionId);
                }
            };
        }
    }
}
