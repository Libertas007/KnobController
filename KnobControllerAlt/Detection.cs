namespace KnobControllerAlt;

public class Detection
{
    private GlobalKeyboardHook _globalKeyboardHook;

    public void Setup()
    {
        _globalKeyboardHook = new GlobalKeyboardHook();

        _globalKeyboardHook.KeyboardPressed += OnKeyPressed;
    }

    private void OnKeyPressed(object sender, GlobalKeyboardHookEventArgs e)
    {
        if (e.KeyboardState == GlobalKeyboardHook.KeyboardState.KeyUp) return;

        Key key = Tools.CodeToKey(e.KeyboardData.VirtualCode);

        if (key == Key.Other) return;
        
        Actions.RunActionForKey(key, Config.meta);
        
        e.Handled = true;
    }
}

public enum Key
{
    Action1,
    Action2,
    Action3,
    Action4,
    Action5,
    Action6,
    Action7,
    Action8,
    Other,
}

public class Tools
{
    public static Key CodeToKey(int code)
    {
        switch (code)
        {
            case 0x80: return Key.Action1;
            case 0x81: return Key.Action2;
            case 0x82: return Key.Action3;
            case 0x83: return Key.Action4;
            case 0x84: return Key.Action5;
            case 0x85: return Key.Action6;
            case 0x86: return Key.Action7;
            case 0x87: return Key.Action8;
            default: return Key.Other;
        }
    }
}