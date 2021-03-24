using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleClient
{
    class KeySender
    {
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern int PostMessage(IntPtr hWnd, UInt32 Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        static extern bool GetCursorPos(out POINT lpPoint);
        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int x, int y);
        [DllImport("user32.dll")]
        static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

        struct POINT
        {
            public int X;
            public int Y;
        }
        public KeySender()
        {

        }

        public void PressKey(int keyCode, bool isKeyboard)
        {
            if(isKeyboard)
            {
                IntPtr hWnd = GetForegroundWindow();
                PostMessage(hWnd, 0x0100, keyCode, 0);
            }
            else
            {
                POINT point = new POINT();
                GetCursorPos(out point);
                mouse_event(keyCode, 0, 0, 0, 0);
            }
        }

        public void MoveMouse(int x, int y)
        {
            POINT point = new POINT();
            GetCursorPos(out point);
            SetCursorPos(point.X + x, point.Y + y);
        }
    }
}
