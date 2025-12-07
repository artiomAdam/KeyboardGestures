namespace KeyboardGestures.Core.Commands
{
    public static class CommandDisplayHelper
    {
        public static string GetDescription(CommandDefinition cmd)
        {
            if (!string.IsNullOrWhiteSpace(cmd.Description)) return cmd.Description;
            return cmd.CommandType switch
            {
                CommandType.LaunchApp => FormatAppName(cmd.ApplicationPath),
                CommandType.LaunchWebpage => $"Launch {cmd.Url}",
                _ => $"{cmd.CommandType}",
            };
        }
        private static string FormatAppName(string? path)
        {
            var name = Path.GetFileNameWithoutExtension(path);

            if (string.IsNullOrWhiteSpace(name))
                return "Launch App";

            // Capitalize safely
            return "Launch " + char.ToUpper(name[0]) + name[1..];
        }

        private static readonly Dictionary<int, string> KnownNames = new()
        {
            { 0x20, "Space" },
            { 0x0D, "Enter" },
            { 0x08, "Backspace" },
            { 0x1B, "Esc" },
            { 0xA0, "LShift" },
            { 0xA1, "RShift" },
            { 0x12, "Alt" },
            { 0xA4, "LAlt" },
            { 0xA5, "RAlt" }
        };

        public static string ToDisplayName(int vk)
        {
            // from lookup
            if (KnownNames.TryGetValue(vk, out var name))
                return name;

            // A-Z
            if (vk >= 'A' && vk <= 'Z')
                return ((char)vk).ToString().ToLower();

            // 0–9
            if (vk >= '0' && vk <= '9')
                return ((char)vk).ToString();

            // symbol keys that map cleanly to chars
            if (vk is >= 0x30 and <= 0x5A)
                return ((char)vk).ToString();

            // f keys
            if (vk >= 0x70 && vk <= 0x87)
                return "F" + (vk - 0x6F);

            // 
            return $"VK_{vk:X2}";
        }
    }
}
