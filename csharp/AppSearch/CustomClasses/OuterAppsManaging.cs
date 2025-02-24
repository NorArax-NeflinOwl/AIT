using System.Diagnostics;
using System.Runtime.InteropServices;

internal class OuterAppsManaging
{
    private readonly string pathToApp = string.Empty;

    private readonly string processName = string.Empty;

    private Process? proc = null;

    public OuterAppsManaging(string path)
    {
        pathToApp = path;
        string text = pathToApp.Substring(pathToApp.LastIndexOf('\\') + 1);
        processName = text.Remove(text.IndexOf('.'));
    }

    public OuterAppsManaging(string path, string processName)
    {
        pathToApp = path;
        this.processName = processName;
    }

    public bool StartApp(out int? output)
    {
        output = null;
        bool flag = false;
        proc = Process.GetProcessesByName(processName).FirstOrDefault();
        if (proc == null)
        {
            Process.Start(pathToApp);
            for (int i = 0; i < 20; i++)
            {
                proc = Process.GetProcessesByName(processName).FirstOrDefault();
                Thread.Sleep(1000);
                if (proc != null)
                {
                    break;
                }
            }
            if (proc != null)
            {
                for (int i = 0; i < 60; i++)
                {
                    Thread.Sleep(500);
                    flag = (proc.MainWindowHandle.ToInt32() != 0);
                    if (flag)
                    {
                        break;
                    }
                }
            }
        }
        else
        {
            flag = (proc.MainWindowHandle.ToInt32() != 0);
        }
        if (flag && proc?.MainWindowHandle != null)
        {
            output = (int)SetForegroundWindow(proc.MainWindowHandle);
            ShowWindowAsync(proc.MainWindowHandle, 10);
        }
        return flag;
    }

    [DllImport("User32.dll")]
    private static extern int SetForegroundWindow(IntPtr point);

    [DllImport("User32.dll")]
    private static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);
}
