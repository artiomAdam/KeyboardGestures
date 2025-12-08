namespace KeyboardGestures.Core.Commands
{
    public class CommandRegistry
    {
        /*
            each command is saved as commandString : CommandDefinition, where the 
            commandString is the sequence with "-"
            e.g: command "ctrl->x->y" is just "x-y": {CommandDefinition}
         */
        private readonly Dictionary<string, CommandDefinition> _commands = new();

        private static string MakeKey(List<int> seq) => string.Join("-", seq);

        public bool Register(CommandDefinition cmd)
        {
            _commands[MakeKey(cmd.Sequence)] = cmd;
            return true;
        }

        public CommandDefinition? FindBySequence(List<int> seq)
            => _commands.TryGetValue(MakeKey(seq), out var cmd) ? cmd : null;

        public IEnumerable<CommandDefinition> GetAll() => _commands.Values;
        public bool Contains(CommandDefinition cmd) => _commands.ContainsValue(cmd);
        public bool ContainsSequence(IEnumerable<int> sequence)
        {
            var key = MakeKey(sequence.ToList());
            return _commands.ContainsKey(key);
        }
        public void Remove(CommandDefinition cmd)
        {
            var key = MakeKey(cmd.Sequence);
            _commands.Remove(key);
        }
        public void UpdateSequence(CommandDefinition cmd, List<int> oldSequence)
        {
            var oldKey = MakeKey(oldSequence);
            _commands.Remove(oldKey);

            Register(cmd);
        }

    }
}
