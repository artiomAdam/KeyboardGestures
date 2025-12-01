using Avalonia;
using System.Runtime.InteropServices;

namespace KeyboardGestures.UI.Utilities
{
    public static class Win32Cursor
    {
        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(out POINT lpPoint);

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;
        }

        public static PixelPoint GetCursorPixelPoint()
        {
            GetCursorPos(out POINT p);
            return new PixelPoint(p.X, p.Y);
        }
    }
}
