using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyboardGestures.Core.Commands
{
    public class CommandDefinition
    {
        public List<int> Sequence { get; set; } = new();
        public string ActionId { get; set; } = "";
    }
}
