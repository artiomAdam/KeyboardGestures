using KeyboardGestures.Core.Events;
using System.Runtime.InteropServices;
namespace KeyboardGestures.Core.KeyboardHook
{
    public class WindowsKeyboardHookService : IKeyboardHookService
    {
        private IntPtr _hookId = IntPtr.Zero;
        private HookProc? _proc;

        public event Action<KeyEvent>? KeyEventReceived;

        public void Start()
        {
            if (_hookId != IntPtr.Zero)
                return;

            _proc = HookCallback;
            _hookId = SetHook(_proc);
        }

        public void Stop()
        {
            if (_hookId != IntPtr.Zero)
            {
                UnhookWindowsHookEx(_hookId);
                _hookId = IntPtr.Zero;
            }
        }

        public void Dispose() => Stop();

        // ----------------------------
        // Win32 Interop
        // ----------------------------

        private static IntPtr SetHook(HookProc proc)
        {
            using var curProcess = System.Diagnostics.Process.GetCurrentProcess();
            using var curModule = curProcess.MainModule!;

            return SetWindowsHookEx(
                WH_KEYBOARD_LL, proc,
                GetModuleHandle(curModule.ModuleName),
                0);
        }

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                var vkCode = Marshal.ReadInt32(lParam);

                var type =
                    wParam == (IntPtr)WM_KEYDOWN ||
                    wParam == (IntPtr)WM_SYSKEYDOWN
                    ? KeyEventType.KeyDown
                    : KeyEventType.KeyUp;

                KeyEventReceived?.Invoke(new KeyEvent(type, vkCode));
            }

            return CallNextHookEx(_hookId, nCode, wParam, lParam);
        }

        private delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);

        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;
        private const int WM_SYSKEYDOWN = 0x0104;
        private const int WM_SYSKEYUP = 0x0105;

        [DllImport("user32.dll")]
        private static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn,
            IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll")]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll")]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
            IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
    }
}
