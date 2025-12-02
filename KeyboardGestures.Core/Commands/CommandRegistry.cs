using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyboardGestures.Core.Commands
{
    public class CommandRegistry
    {
        private readonly Dictionary<string, CommandDefinition> _commands = new();

        private static string MakeKey(List<int> seq) => string.Join("-", seq);

        public void Register(CommandDefinition cmd)
        {
            _commands[MakeKey(cmd.Sequence)] = cmd;
        }

        public CommandDefinition? FindBySequence(List<int> seq)
        {
            _commands.TryGetValue(MakeKey(seq), out var cmd);
            return cmd;
        }

        public IEnumerable<CommandDefinition> GetAll() => _commands.Values;
    }
}
