namespace KeyboardGestures.Core.Commands
{
    public interface ICommandService
    {
        IEnumerable<CommandDefinition> LoadAll();

        void AddNew(CommandDefinition cmd);
        
        void UpdateCommand(CommandDefinition cmd, List<int>? oldSequence = null);
        void Delete(CommandDefinition cmd);
    }
}
