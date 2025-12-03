namespace KeyboardGestures.Core.Commands
{
    public static class CommandDescriptionHelper
    {
        public static string GetDescription(CommandDefinition cmd)
        {
            if (!string.IsNullOrWhiteSpace(cmd.Description)) return cmd.Description;
            return cmd.CommandType switch
            {
                CommandType.LaunchApp => $"Launch {System.IO.Path.GetFileNameWithoutExtension(cmd.ApplicationPath)}",
                _ => "Not implemented yet",
            };
        }
    }
}
