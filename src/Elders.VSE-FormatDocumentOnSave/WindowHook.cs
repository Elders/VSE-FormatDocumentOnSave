using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Elders.VSE_FormatDocumentOnSave
{
    abstract class WindowHook
    {
        [DllImport("user32.dll")]
        private static extern IntPtr SetWindowsHookEx(HookType hookType, HookProc callback, IntPtr mod, uint threadId);

        [DllImport("user32.dll")]
        private static extern bool UnhookWindowsHookEx(IntPtr hookHandle);

        [DllImport("user32.dll")]
        protected static extern int CallNextHookEx(IntPtr hookHandle, int code, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll")]
        private static extern uint GetCurrentThreadId();

        protected IntPtr _hookHandle = IntPtr.Zero;

        protected delegate int HookProc(int code, IntPtr wParam, IntPtr lParam);

        protected HookProc _hookProc;

        protected enum HookType : uint
        {
            WH_JOURNALRECORD = 0,
            WH_JOURNALPLAYBACK = 1,
            WH_KEYBOARD = 2,
            WH_GETMESSAGE = 3,
            WH_CALLWNDPROC = 4,
            WH_CBT = 5,
            WH_SYSMSGFILTER = 6,
            WH_MOUSE = 7,
            WH_HARDWARE = 8,
            WH_DEBUG = 9,
            WH_SHELL = 10,
            WH_FOREGROUNDIDLE = 11,
            WH_CALLWNDPROCRET = 12,
            WH_KEYBOARD_LL = 13,
            WH_MOUSE_LL = 14,
        }

        protected HookType _hookType;

        public void Install()
        {
            if (_hookHandle == IntPtr.Zero)
                _hookHandle = SetWindowsHookEx(_hookType, _hookProc, IntPtr.Zero, GetCurrentThreadId());
        }

        public void Uninstall()
        {
            if (_hookHandle != IntPtr.Zero)
            {
                UnhookWindowsHookEx(_hookHandle);
                _hookHandle = IntPtr.Zero;
            }
        }
    }

    class WindowKeyboardHook : WindowHook
    {
        public WindowKeyboardHook()
        {
            _hookType = HookType.WH_KEYBOARD;
            _hookProc = HookProcedure;
        }

        public delegate void MessageEvent(Keys key, bool isPressing);

        public event MessageEvent OnMessage;

        private int HookProcedure(int code, IntPtr wParam, IntPtr lParam)
        {
            if (code != 0)
                return CallNextHookEx(_hookHandle, code, wParam, lParam);

            if (OnMessage != null && wParam != IntPtr.Zero)
                OnMessage.Invoke((Keys)wParam, ((ulong)lParam & 0x80000000) == 0);

            return CallNextHookEx(_hookHandle, code, wParam, lParam);
        }
    }
}
