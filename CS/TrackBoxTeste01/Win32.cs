using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.InteropServices;
using System.Drawing;

namespace TrackBoxTeste01
{
    class Win32
    {
        //[Flags()]
        //internal enum MOUSEEVENTF : int
        //{
        //    MOVE = 0x1,
        //    LEFTDOWN = 0x2,
        //    LEFTUP = 0x4,
        //    RIGHTDOWN = 0x8,
        //    RIGHTUP = 0x10,
        //    MIDDLEDOWN = 0x20,
        //    MIDDLEUP = 0x40,
        //    XDOWN = 0x80,
        //    XUP = 0x100,
        //    VIRTUALDESK = 0x400,
        //    WHEEL = 0x800,
        //    ABSOLUTE = 0x8000
        //}

        [DllImport("User32.Dll")]
        public static extern long SetCursorPos(int x, int y);

        [DllImport("User32.Dll")]
        public static extern bool ClientToScreen(IntPtr hWnd, ref POINT point);

        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out POINT lpPoint);

        [DllImport("user32.dll")]
        public static extern bool GetKeyboardState(byte[] lpKeyState);

        [DllImport("User32.Dll")]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);

        public static void MouseLeftDown(uint x, uint y)
        {
            mouse_event(0x2, x, y, 0, 0);
        }

        public static void MouseLeftUp(uint x, uint y)
        {
            mouse_event(0x4, x, y, 0, 0);
        }
        public static void MouseRightDown(uint x, uint y)
        {
            mouse_event(0x8, x, y, 0, 0);
        }

        public static void MouseRightUp(uint x, uint y)
        {
            mouse_event(0x10, x, y, 0, 0);
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int x;
            public int y;
        }

        static public void SetMousePoint(int x, int y)
        {
            POINT p = new POINT();
            //GetCursorPos(out p);
            p.x += Convert.ToInt16(x);
            p.y += Convert.ToInt16(y);
            SetCursorPos(p.x, p.y);
        }

        static public Point GetMousePoint()
        {
            POINT p = new POINT();
            GetCursorPos(out p);
            Point p2 = new Point(p.x, p.y);
            return p2;
        }

        static public bool IsKeyPressed()
        {
            bool result = false;
            byte[] keyboardState = new byte[256];
            GetKeyboardState(keyboardState);

            for (var index = 0; index < 256; index++)
            {
                if ((keyboardState[index] & 0x80) != 0)
                {
                    result = true;
                }
            }
            return result;
        }




    }
}
