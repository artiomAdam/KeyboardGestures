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
            { 0x10, "Shift" },
            { 0xA0, "Shift" },
            { 0xA1, "Shift" },
            { 0x11, "Ctrl" },
            { 0xA2, "Ctrl" },
            { 0xA3, "Ctrl" },
            { 0x12, "Alt" },
            { 0xA4, "Alt" },
            { 0xA5, "Alt" },
            { 0xBA, ";" },
            { 0xDE, "'" },
            { 0xC0, "`" },
            { 0xDB, "[" },
            { 0xDD, "]" },
            { 0xBF, "/" },
            { 0xDC, "\\" },
            { 0xBB, "=" },
            { 0xBD, "-" },
            { 0x25, "Arrow_Left" },
            { 0x26, "Arrow_Up"},
            { 0x27, "Arrow_Right"},
            { 0x28, "Arrow_down"},
            { 0x2D, "Insert_Key" },
            { 0x2E, "Delete_Key" },
            { 0x09, "Tab" },
            { 0X14, "Capslock" },
            { 0x60, "Numpad_0" },
            { 0x61, "Numpad_1" },
            { 0x62, "Numpad_2" },
            { 0x63, "Numpad_3" },
            { 0x64, "Numpad_4" },
            { 0x65, "Numpad_5" },
            { 0x66, "Numpad_6" },
            { 0x67, "Numpad_7" },
            { 0x68, "Numpad_8" },
            { 0x69, "Numpad_9" },
            { 0x6F, "Numpad_/" },
            { 0x6A, "Numpad_*" },
            { 0x6D, "Numpad_-" },
            { 0x6B, "Numpad_+" },
            { 0x6E, "Numpad_." },


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
