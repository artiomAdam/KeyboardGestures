namespace KeyboardGestures.Core.Commands
{
    public record CommandRule(bool SingleInstance, int Order);

    public static class CommandRules
    {
        private static readonly Dictionary<CommandType, CommandRule> _rules =
            new()
            {
            { CommandType.ToggleMute,     new(true, 0) },
            { CommandType.TakeScreenshot,new(true, 0) },
            { CommandType.CopyCurrentPath,new(true, 0) },
            { CommandType.LaunchApp,      new(false, 1) },
            { CommandType.LaunchWebpage,  new(false, 2) },
            };

        public static bool IsSingleInstance(CommandType type)
            => _rules.TryGetValue(type, out var rule) && rule.SingleInstance;

        public static int GetOrder(CommandType type)
            => _rules.TryGetValue(type, out var rule) ? rule.Order : 99;
    }
}
