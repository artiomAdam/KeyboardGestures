namespace KeyboardGestures.Core.Commands
{
    public enum CommandType
    {
        LaunchApp
    }
    public class CommandDefinition
    {
        public List<int> Sequence { get; set; } = new();
        public CommandType CommandType { get; set; }

        public string? Description { get; set; }
        public string? ApplicationPath { get; set; } // only for LaunchApp type
    }
}
