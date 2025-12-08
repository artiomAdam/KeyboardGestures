using System.Runtime.InteropServices;

namespace KeyboardGestures.Core.Utilities
{
    public static class SystemAudioHelper
    {
        private const uint WM_APPCOMMAND = 0x0319;
        private const int APPCOMMAND_VOLUME_MUTE = 0x80000; // APPCOMMAND_VOLUME_MUTE << 16

        public static void ToggleMute()
        {
            // try send to foreground window if none, send to desktop
            var hWnd = GetForegroundWindow();
            if (hWnd == IntPtr.Zero)
            {
                hWnd = GetDesktopWindow();
            }
            
            if (hWnd != IntPtr.Zero)
            {
                // wParam is the window handle, lParam has the APPCOMMAND in the high word
                SendMessageW(hWnd, WM_APPCOMMAND, hWnd, (IntPtr)APPCOMMAND_VOLUME_MUTE);
            }
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr SendMessageW(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
    }

}
