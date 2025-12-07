namespace KeyboardGestures.Core.Commands
{
    public interface ICommandService
    {
        IEnumerable<CommandDefinition> LoadAll();

        void AddNew(CommandDefinition cmd);
        void UpdateSequence(CommandDefinition cmd, List<int> newSequence);
        void UpdateCommand(CommandDefinition cmd);
        void Delete(CommandDefinition cmd);
    }
}
