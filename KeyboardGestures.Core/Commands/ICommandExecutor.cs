namespace KeyboardGestures.Core.Commands
{
    public interface ICommandExecutor
    {
        void Execute(CommandDefinition command);
    }
}
