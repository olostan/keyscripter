using System.ComponentModel;
using System.Threading;

namespace Scripter
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Windows.Forms;

    // ReSharper disable InconsistentNaming
    #pragma warning disable 169 

    public class Funcs
    {

        private const int GW_OWNER = 4;
        private const int GWL_EXSTYLE = -20;
        private const int INPUT_HARDWARE = 2;
        private const int INPUT_KEYBOARD = 1;
        private const int INPUT_MOUSE = 0;
        private const uint KEYEVENTF_EXTENDEDKEY = 1;
        private const uint KEYEVENTF_KEYUP = 2;
        private const uint KEYEVENTF_SCANCODE = 8;
        private const uint KEYEVENTF_UNICODE = 4;
        private const uint MOUSEEVENTF_ABSOLUTE = 0x8000;
        private const uint MOUSEEVENTF_LEFTDOWN = 2;
        private const uint MOUSEEVENTF_LEFTUP = 4;
        private const uint MOUSEEVENTF_MIDDLEDOWN = 0x20;
        private const uint MOUSEEVENTF_MIDDLEUP = 0x40;
        private const uint MOUSEEVENTF_MOVE = 1;
        private const uint MOUSEEVENTF_RIGHTDOWN = 8;
        private const uint MOUSEEVENTF_RIGHTUP = 0x10;
        private const uint MOUSEEVENTF_VIRTUALDESK = 0x4000;
        private const uint MOUSEEVENTF_WHEEL = 0x800;
        private const uint MOUSEEVENTF_XDOWN = 0x80;
        private const uint MOUSEEVENTF_XUP = 0x100;
        public const int WA_INACTIVE = 0;
        public const int WA_ACTIVE = 1;
        public const int WA_CLICKACTIVE = 2;
        public const uint WM_ACTIVATE = 6;
        public const uint WM_SETFOCUS = 7;
        public const uint WM_ACTIVATEAPP = 0x1c;
        public const uint WM_CHAR = 0x102;
        private const int WM_KEYDOWN = 0x100;
        private const int WM_KEYUP = 0x101;
        private const int WS_EX_APPWINDOW = 0x40000;
        private const int WS_EX_TOOLWINDOW = 0x80;
        private const uint XBUTTON1 = 1;
        private const uint XBUTTON2 = 2;

        public static bool CheckValidWindow(uint hwnd)
        {
            uint num;
            return (GetWindowThreadProcessId(new IntPtr(hwnd), out num) > 0);
        }

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("kernel32.dll")]
        private static extern bool CloseHandle(IntPtr hObject);
        [DllImport("user32")]
        private static extern int EnumWindows(EnumWindowsProcDelegate lpEnumFunc, int lParam);
        [DllImport("user32")]
        private static extern int GetDesktopWindow();
        private static EnumWindowsProcDelegate GetEnumerateDelegate(ICollection<uint> list)
        {
            return delegate (int hWnd, int lParem) {
                if (IsTaskbarWindow(hWnd))
                {
                    string windowModuleFileName = GetWindowModuleFileName(new IntPtr(hWnd));
                    Debug.WriteLine("Found window with process:" + windowModuleFileName);
                    if (windowModuleFileName.EndsWith("notepad.exe", true, null))
                    {
                        list.Add((uint) hWnd);
                    }
                }
                return 1;
            };
        }

        [DllImport("user32.dll")]
        public static extern int GetForegroundWindow();
        [DllImport("psapi.dll")]
        private static extern uint GetModuleFileNameEx(IntPtr hProcess, IntPtr hModule, [Out] StringBuilder lpBaseName, [In, MarshalAs(UnmanagedType.U4)] int nSize);
        [DllImport("user32")]
        private static extern int GetParent(int hwnd);
        public static uint[] GetRFWindows()
        {
            List<uint> list = new List<uint>();
            EnumWindows(GetEnumerateDelegate(list), 0);
            return list.ToArray();
        }

        [DllImport("user32")]
        private static extern int GetWindow(int hwnd, int wCmd);
        [DllImport("user32", EntryPoint="GetWindowLongA")]
        private static extern int GetWindowLongPtr(int hwnd, int nIndex);
        private static string GetWindowModuleFileName(IntPtr hWnd)
        {
            uint num;
            StringBuilder lpBaseName = new StringBuilder(0x400);
            GetWindowThreadProcessId(hWnd, out num);
            IntPtr hProcess = OpenProcess(0x410, 0, num);
            GetModuleFileNameEx(hProcess, IntPtr.Zero, lpBaseName, 0x400);
            CloseHandle(hProcess);
            return lpBaseName.ToString();
        }

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
        private static bool IsTaskbarWindow(int hWnd)
        {
            int windowLongPtr = GetWindowLongPtr(hWnd, -20);
            int parent = GetParent(hWnd);
            bool flag = ((IsWindowVisible(hWnd) != 0) & (GetWindow(hWnd, 4) == 0)) & ((parent == 0) | (parent == GetDesktopWindow()));
            if ((windowLongPtr & 0x80) == 0x80)
            {
                flag = false;
            }
            if ((windowLongPtr & 0x40000) == 0x40000)
            {
                flag = true;
            }
            return flag;
        }

        [DllImport("user32")]
        private static extern int IsWindowVisible(int hwnd);
        public static void MouseClick(uint targetWindow, int btn)
        {
            PostMessageSafe(targetWindow, 6, 0, 0);
            var input = new INPUT
                            {
                                type = 0,
                                mi = new MOUSEINPUT
                                         {
                                             dwFlags = MouseEventFByButton(btn, true),
                                             dwExtraInfo = 0,
                                             mouseData = 0,
                                             time = 0
                                         }
                            };
            var num = SendInput(1, ref input, Marshal.SizeOf(input));
            Trace.WriteLine("result:" + num);
            if (num != 1)
            {
                MessageBox.Show("Не получилось нажать кнопку мышки: " + Marshal.GetLastWin32Error());
                Marshal.ThrowExceptionForHR(Marshal.GetLastWin32Error());
            }
            input.mi.dwFlags = MouseEventFByButton(btn, false);
            Trace.WriteLine("result:" + SendInput(1, ref input, Marshal.SizeOf(input)));
        }

        private static uint MouseEventFByButton(int btn, bool press)
        {
            switch (btn)
            {
                case 0:
                    return (press ? 2u : 4);

                case 1:
                    return (press ? 8u : 0x10);

                case 2:
                    return (press ? 0x20u : 0x40);
            }
            throw new NotSupportedException("unknown button");
        }

        [DllImport("kernel32.dll")]
        private static extern IntPtr OpenProcess(uint dwDesiredAccess, int bInheritHandle, uint dwProcessId);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool PostMessage(HandleRef hWnd, uint Msg, IntPtr wParam,IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool DefWindowProc(HandleRef hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        private static void PostMessageSafe(uint hWnd, uint msg, uint wParam, uint lParam)
        {
            string wrapper = "asd";
            bool returnValue = PostMessage(new HandleRef(wrapper,new IntPtr(hWnd)), msg, new IntPtr(wParam), new IntPtr(lParam));
            if (!returnValue)
            {
                // An error occured
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }

        private static void DefWindowProcSafe(uint hWnd, uint msg, uint wParam, uint lParam)
        {
            string wrapper = "asd";
            PostMessageSafe(hWnd, msg, wParam, lParam);
            bool returnValue = DefWindowProc(new HandleRef(wrapper, new IntPtr(hWnd)), msg, new IntPtr(wParam), new IntPtr(lParam));
            if (!returnValue)
            {
                // An error occured
                //throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }    

        [DllImport("User32.dll")]
        private static extern uint SendInput(uint numberOfInputs, ref INPUT input, int structSize);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern uint MapVirtualKey(uint uCode, int nMapType);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);
        
        public static void SendKeyToWindow(uint hWnd, uint code)
        {
           //Trace.WriteLine("Sending "+code+" to "+hWnd);            

            SendMessage(hWnd, WM_ACTIVATE, WA_ACTIVE, 0);
            
            
            SendMessage(hWnd, WM_KEYDOWN, code, 1 + (MapVirtualKey(code, 0) << 16));
            //Thread.Sleep(150);
            //SendMessage(hWnd, WM_KEYUP, code, DownBase + (MapVirtualKey(code, 0) << 16 + 3 << 30));
            //Thread.Sleep(50);
            SendMessage(hWnd, WM_ACTIVATE, WA_INACTIVE, 0);
            //SetForegroundWindow(new IntPtr(hwnd));
        }

        [DllImport("user32.dll")]
        public static extern int SendMessage(uint hWnd, uint Msg, uint wParam, uint lParam);

        private delegate int EnumWindowsProcDelegate(int hWnd, int lParam);

        [StructLayout(LayoutKind.Sequential)]
        internal struct HARDWAREINPUT
        {
            public int uMsg;
            public short wParamL;
            public short wParamH;
        }

        [StructLayout(LayoutKind.Explicit)]
        internal struct INPUT
        {
            [FieldOffset(4)]
            public HARDWAREINPUT hi;
            [FieldOffset(4)]
            public KEYBDINPUT ki;
            [FieldOffset(4)]
            public MOUSEINPUT mi;
            [FieldOffset(0)]
            public int type;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct KEYBDINPUT
        {
            private short wVk;
            private short wScan;
            private int dwFlags;
            private int time;
            private IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct MOUSEINPUT
        {
            [FieldOffset(20)]
            public uint dwExtraInfo;
            [FieldOffset(12)]
            public uint dwFlags;
            [FieldOffset(0)]
            public uint dx;
            [FieldOffset(4)]
            public uint dy;
            [FieldOffset(8)]
            public uint mouseData;
            [FieldOffset(0x10)]
            public uint time;
        }

        [DllImport("user32.dll")]
        public static extern int GetWindowText(int hWnd, StringBuilder text, int count);
 

    }
    // ReSharper restore InconsistentNaming
    #pragma warning restore 169
}

