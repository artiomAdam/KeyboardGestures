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

        public IEnumerable<DisplayCommand> GetDisplayCommands()
        {
            foreach(var cmd in _commands.Values)
            {
                yield return new DisplayCommand
                {
                    SequenceText = string.Join(" ", cmd.Sequence.Select(x => x.ToString("X2"))),
                    Description = CommandDescriptionHelper.GetDescription(cmd)
                };
            }
        }
    }
}
