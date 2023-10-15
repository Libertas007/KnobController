using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using WindowsInput;
using WindowsInput.Native;

namespace KnobControllerAlt;

public class Actions
{
    private static bool hand = false;
    
    public static void RunActionForKey(Key key, bool meta)
    {
        switch (key)
        {
            case Key.Action1: RunActionKey1(meta); return;
            case Key.Action2: RunActionKey2(meta); return;
            case Key.Action3: RunActionKey3(meta); return;
            case Key.Action4: RunActionKey4(meta); return;
            case Key.Action5: RunActionKey5(meta); return;
            case Key.Action6: RunActionKey6(meta); return;
            case Key.Action7: RunActionKey7(meta); return;
            case Key.Action8: RunActionKey8(meta); return;
        }
    }
    
    public static void RunActionKey1(bool meta)
    {
        // microphone

        var windows = GenericActions.GetOpenWindows().Values.ToList();

        string? discord = windows.Find(s => s.Contains("Discord"));

        if (discord != null)
        {
            GenericActions.SendKeysTo(discord, new [] { VirtualKeyCode.CONTROL, VirtualKeyCode.SHIFT}, VirtualKeyCode.VK_M);
        }
    }
    
    public static void RunActionKey2(bool meta)
    {
        // camera
    }
    
    public static void RunActionKey3(bool meta)
    {
        // hand
        
        if (meta) return;

        hand = !hand;

        if (hand)
        {
            Controller.ChangeLED(2, Color.YELLOW, Effects.LIGHT);
        }
        else
        {
            Controller.ChangeLED(2, Color.WHITE, Effects.LIGHT);
        }
    }
    
    public static void RunActionKey4(bool meta)
    {
        // meta
        Config.SetMeta(!Config.meta);
        Config.SaveData();
        
        if (Config.meta)
        {
            Controller.ChangeLED(3, Color.BLUE, Effects.BLINK);
        }
        else
        {
            Controller.ChangeLED(3, Color.BLUE, Effects.LIGHT);
        }
    }
    
    public static void RunActionKey5(bool meta)
    {
        // windows, not in use
    }
    
    public static void RunActionKey6(bool meta)
    {
        // deepl
        GenericActions.SendKeys(new [] { VirtualKeyCode.CONTROL }, VirtualKeyCode.F9);
    }
    
    public static void RunActionKey7(bool meta)
    {
        // desktop
        GenericActions.SendKeys(new []{VirtualKeyCode.LWIN}, VirtualKeyCode.VK_D);
    }
    
    public static void RunActionKey8(bool meta)
    {
        // end call
    }
}

public class GenericActions
{
    public static void ActivateWindow(string name)
    {
        var allProcesses = Process.GetProcesses();

        var processes = allProcesses.Where((process1, i) => process1.MainWindowTitle == name).ToList();

        if (processes.Count > 0) {
            SetForegroundWindow(processes[0].MainWindowHandle);
        }
        else
        {
            MessageBox.Show("Cannot find process: " + name);
        }
    }

    public static void SendKeys(IEnumerable<VirtualKeyCode> modifierKeyCodes, VirtualKeyCode keyCode)
    {
        InputSimulator sim = new InputSimulator();
        sim.Keyboard.ModifiedKeyStroke(modifierKeyCodes, keyCode);
    }
    
    public static void SendKeysTo(string name, IEnumerable<VirtualKeyCode> modifierKeyCodes, VirtualKeyCode keyCode)
    {
        IntPtr current = GetForegroundWindow();
        Thread.Sleep(1);
        ActivateWindow(name);
        Thread.Sleep(1);
        SendKeys(modifierKeyCodes, keyCode);
        Thread.Sleep(1);
        SetForegroundWindow(current);

    }
    
    public static string GetActiveWindowTitle()
    {
        const int nChars = 256;
        StringBuilder buff = new StringBuilder(nChars);
        IntPtr handle = GetForegroundWindow();

        if (GetWindowText(handle, buff, nChars) > 0)
        {
            return buff.ToString();
        }
        return "";
    }
    
    public static string GetActiveProcessFileName()
    {
        IntPtr hwnd = GetForegroundWindow();
        uint pid;
        GetWindowThreadProcessId(hwnd, out pid);
        Process p = Process.GetProcessById((int)pid);
        return p.MainModule?.FileName ?? "";
    }
    
    public static IDictionary<IntPtr, string> GetOpenWindows()
    {
        IntPtr shellWindow = GetShellWindow();
        Dictionary<IntPtr, string> windows = new Dictionary<IntPtr, string>();

        EnumWindows(delegate(IntPtr hWnd, int lParam)
        {
            if (hWnd == shellWindow) return true;
            if (!IsWindowVisible(hWnd)) return true;

            int length = GetWindowTextLength(hWnd);
            if (length == 0) return true;

            StringBuilder builder = new StringBuilder(length);
            GetWindowText(hWnd, builder, length + 1);

            windows[hWnd] = builder.ToString();
            return true;

        }, 0);

        return windows;
    }

    private delegate bool EnumWindowsProc(IntPtr hWnd, int lParam);

    [DllImport("USER32.DLL")]
    private static extern bool EnumWindows(EnumWindowsProc enumFunc, int lParam);

    [DllImport("USER32.DLL")]
    private static extern int GetWindowTextLength(IntPtr hWnd);

    [DllImport("USER32.DLL")]
    private static extern bool IsWindowVisible(IntPtr hWnd);

    [DllImport("USER32.DLL")]
    private static extern IntPtr GetShellWindow();
    
    [DllImport("user32.dll")]
    public static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, out uint processId);
    
    [DllImport("user32.dll")]
    private static extern bool SetForegroundWindow(IntPtr hWnd);
    
    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    private static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);
}
