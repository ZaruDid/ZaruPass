using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Automation;

namespace ZaruPass.Core
{
    public class WindowHelper
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

        [DllImport("user32.dll")]
        private static extern bool EnumWindows(EnumWindowsProc enumProc, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern bool IsWindowVisible(IntPtr hWnd);

        private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        public static List<IntPtr> GetVisibleWindows()
        {
            var windows = new List<IntPtr>();
            EnumWindows((hWnd, lParam) =>
            {
                if (IsWindowVisible(hWnd))
                {
                    var sb = new System.Text.StringBuilder(256);
                    GetWindowText(hWnd, sb, 256);
                    if (!string.IsNullOrEmpty(sb.ToString()))
                    {
                        windows.Add(hWnd);
                    }
                }
                return true;
            }, IntPtr.Zero);
            return windows;
        }

        public static string GetActiveWindowTitle()
        {
            IntPtr foregroundWindow = GetForegroundWindow();

            return GetWindowTitile(foregroundWindow);
        }

        public static string GetWindowTitile(IntPtr window)
        {
            GetWindowThreadProcessId(window, out uint processId);
            Process process = Process.GetProcessById((int)processId);
            return process.ProcessName.ToLower();
        }

        public static string GetBrowserUrl()
        {
            IntPtr foregroundWindow = GetForegroundWindow();

            if (foregroundWindow == IntPtr.Zero)
                return null;

            GetWindowThreadProcessId(foregroundWindow, out uint processId);
            Process process = Process.GetProcessById((int)processId);

            string processName = process.ProcessName.ToLower();

            return GetChromeUrl(foregroundWindow);
        }

        private static string GetChromeUrl(IntPtr hWnd)
        {
            try
            {
                AutomationElement element = AutomationElement.FromHandle(hWnd);
                if (element == null)
                    return null;

                // Ищем поле адреса в Chrome
                Condition condition = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Edit);

                AutomationElement addressBar = element.FindFirst(
                    TreeScope.Descendants,
                    condition
                );

                if (addressBar == null)
                    return null;

                // Получаем значение поля
                object valuePattern;
                if (addressBar.TryGetCurrentPattern(ValuePattern.Pattern, out valuePattern))
                {
                    return ((ValuePattern)valuePattern).Current.Value;
                }

                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}