using KeyboardGestures.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyboardGestures.Core.Gestures
{
    public interface IGestureInterpreter
    {
        void OnKeyEvent(KeyEvent ev);
    }
}
