using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyboardGestures.Core.Commands
{
    public interface ICommandExecutor
    {
        void Execute(string actionId);
    }
}
