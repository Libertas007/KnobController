using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using WindowsInput;
using WindowsInput.Native;

namespace KnobControllerAlt;

public class Actions
{
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
    }
    
    public static void RunActionKey2(bool meta)
    {
        // camera
    }
    
    public static void RunActionKey3(bool meta)
    {
        // hand
    }
    
    public static void RunActionKey4(bool meta)
    {
        // meta
        Config.SetMeta(!meta);
        Config.SaveData();
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
    public static void ActivateWindow(string name) {
        var processes = Process.GetProcessesByName(name);
        if (processes.Length > 0) {
            SetForegroundWindow(processes[0].MainWindowHandle);
        }
    }

    public static void SendKeys(IEnumerable<VirtualKeyCode> modifierKeyCodes, VirtualKeyCode keyCode)
    {
        InputSimulator sim = new InputSimulator();
        sim.Keyboard.ModifiedKeyStroke(modifierKeyCodes, keyCode);
    }
    
    public static void SendKeysTo(string name, IEnumerable<VirtualKeyCode> modifierKeyCodes, VirtualKeyCode keyCode)
    {
        Process current = Process.GetCurrentProcess();
        Console.WriteLine(current.ProcessName);
        Thread.Sleep(20);
        ActivateWindow(name);
        SendKeys(modifierKeyCodes, keyCode);
        Thread.Sleep(20);
        SetForegroundWindow(current.MainWindowHandle);
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
    
    [DllImport("user32.dll")]
    public static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, out uint processId);
    
    [DllImport("user32.dll")]
    private static extern bool SetForegroundWindow(IntPtr hWnd);
    
    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    private static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);
}
