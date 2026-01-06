using KeyboardGestures.Core.Commands;
using KeyboardGestures.Core.Utilities;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text;

namespace KeyboardGestures.UI.ExecutionPlatform
{
    public class WinCommandExecutionAdapter : ICommandExecutionService
    {

        public Task LaunchApp(string? path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return Task.CompletedTask;

            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = path,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"LaunchApp error: {ex}");
            }

            return Task.CompletedTask;
        }


        public Task LaunchWebpage(string? url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return Task.CompletedTask;

            return Task.Run(() =>
            {
                try
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = url,
                        UseShellExecute = true
                    });
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"LaunchWebpage error: {ex}");
                }
            });
        }

        public Task CopyCurrentPath()
        {
            var text = WindowContextHelper.GetCurrentContextPath();
            if (string.IsNullOrWhiteSpace(text))
                return Task.CompletedTask;

            return Task.Run(() =>
            {
                var bytes = Encoding.Unicode.GetBytes(text + "\0");
                IntPtr ptr = Marshal.AllocHGlobal(bytes.Length);
                Marshal.Copy(bytes, 0, ptr, bytes.Length);

                OpenClipboard(IntPtr.Zero);
                EmptyClipboard();
                SetClipboardData(CF_UNICODETEXT, ptr);
                CloseClipboard();
            });
        }



        [DllImport("user32.dll", SetLastError = true)]
        static extern bool OpenClipboard(IntPtr hWndNewOwner);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool CloseClipboard();

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool EmptyClipboard();

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr SetClipboardData(uint format, IntPtr handle);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        static extern uint RegisterClipboardFormat(string lpszFormat);
        const uint CF_DIB = 8;
        const uint CF_UNICODETEXT = 13;
        static readonly uint CF_PNG = RegisterClipboardFormat("PNG");
        [DllImport("user32.dll")]
        static extern int GetSystemMetrics(int nIndex);

        const int SM_CXSCREEN = 0;
        const int SM_CYSCREEN = 1;


        public Task TakeScreenshot()
        {
            return Task.Run(() =>
            {
                int width = GetSystemMetrics(SM_CXSCREEN);
                int height = GetSystemMetrics(SM_CYSCREEN);

                using var bmp = new System.Drawing.Bitmap(width, height);
                using (var g = Graphics.FromImage(bmp))
                    g.CopyFromScreen(0, 0, 0, 0, bmp.Size);

                // PNG
                byte[] pngBytes;
                using (var ms = new MemoryStream())
                {
                    bmp.Save(ms, ImageFormat.Png);
                    pngBytes = ms.ToArray();
                }

                // DIB
                byte[] dibBytes = BitmapToDib(bmp);

                // Clipboard
                OpenClipboard(IntPtr.Zero);
                EmptyClipboard();

                // CF_DIB
                IntPtr dibPtr = Marshal.AllocHGlobal(dibBytes.Length);
                Marshal.Copy(dibBytes, 0, dibPtr, dibBytes.Length);
                SetClipboardData(CF_DIB, dibPtr);

                // CF_PNG
                IntPtr pngPtr = Marshal.AllocHGlobal(pngBytes.Length);
                Marshal.Copy(pngBytes, 0, pngPtr, pngBytes.Length);
                SetClipboardData(CF_PNG, pngPtr);

                CloseClipboard();

            });
        }

        private static byte[] BitmapToDib(System.Drawing.Bitmap bmp)
        {
            var rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            var data = bmp.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            int stride = data.Stride;
            int bytes = stride * bmp.Height;

            byte[] pixelData = new byte[bytes];
            Marshal.Copy(data.Scan0, pixelData, 0, bytes);
            bmp.UnlockBits(data);


            //    typedef struct tagBITMAPINFOHEADER
            //    {
            //      DWORD biSize;
            //      LONG biWidth;
            //      LONG biHeight;
            //      WORD biPlanes;
            //      WORD biBitCount;
            //      DWORD biCompression;
            //      DWORD biSizeImage;  - 0 for uncompressed
            //      LONG biXPelsPerMeter;   - no.
            //      LONG biYPelsPerMeter;   - no.
            //      DWORD biClrUsed;  - color table
            //      DWORD biClrImportant;  - color table
            //    }
            // 40-byte BITMAPINFOHEADER
            byte[] header = new byte[40];

            BitConverter.GetBytes(40).CopyTo(header, 0);       // biSize
            BitConverter.GetBytes(bmp.Width).CopyTo(header, 4); // biWidth
            BitConverter.GetBytes(-bmp.Height).CopyTo(header, 8); //biHeight
            BitConverter.GetBytes((short)1).CopyTo(header, 12);  // biPlanes
            BitConverter.GetBytes((short)32).CopyTo(header, 14); // biBitCount
            BitConverter.GetBytes(0).CopyTo(header, 16);         // BI_RGB

            // Combine header + pixels
            byte[] dib = new byte[header.Length + pixelData.Length];
            Buffer.BlockCopy(header, 0, dib, 0, header.Length);
            Buffer.BlockCopy(pixelData, 0, dib, header.Length, pixelData.Length);

            return dib;
        }


        public Task ToggleMute()
        {
            try
            {
                SystemAudioHelper.ToggleMute();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ToggleMute error: {ex}");
            }

            return Task.CompletedTask;
        }
    }
}
