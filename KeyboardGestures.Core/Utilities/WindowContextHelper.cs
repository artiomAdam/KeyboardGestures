
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace KeyboardGestures.Core.Utilities
{
    public static class WindowContextHelper
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        public static string? GetCurrentContextPath()
        {
            var hWnd = GetForegroundWindow();
            if (hWnd == IntPtr.Zero)
                return null;

            var sb = new StringBuilder(512);
            GetWindowText(hWnd, sb, sb.Capacity);
            var title = sb.ToString();

            if (string.IsNullOrWhiteSpace(title))
                return null;

            // -----------------------------
            // 1) Starts with a Windows path? Handle Explorer path correctly
            // -----------------------------
            if (title.Length > 3 &&
                char.IsLetter(title[0]) &&
                title[1] == ':' &&
                title[2] == '\\')
            {
                return CutAfterSeparator(title);
            }

            // -----------------------------
            // 2) Browser titles: strip after dash (en/em/etc.)
            // -----------------------------
            return CutAfterSeparator(title);
        }

        private static string CutAfterSeparator(string title)
        {
            // All separators used by Windows apps
            string[] separators = { " - ", " — ", " – " };

            foreach (var sep in separators)
            {
                var idx = title.IndexOf(sep, StringComparison.Ordinal);
                if (idx > 0)
                    return title[..idx];
            }

            return title;
        }
    }
}
