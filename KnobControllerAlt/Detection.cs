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
        
        MessageBox.Show($@"flags: {e.KeyboardData.Flags}    key: {e.KeyboardData.VirtualCode}   state: {e.KeyboardState}");
        e.Handled = true;

        /*if (e.KeyboardData.Flags == GlobalKeyboardHook.KfAltdown)
        {

            MessageBox.Show($@"{(Keys)e.KeyboardData.VirtualCode} has been pressed.", "Key pressed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            e.Handled = true;
        }*/
    }
}

public enum Keys
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
    public static Keys CodeToKey(int code)
    {
        switch (code)
        {
            case 0x61: return Keys.Action1;
            case 0x62: return Keys.Action2;
            case 0x63: return Keys.Action3;
            case 0x64: return Keys.Action4;
            case 0x65: return Keys.Action5;
            case 0x66: return Keys.Action6;
            case 0x67: return Keys.Action7;
            case 0x68: return Keys.Action8;
            default: return Keys.Other;
        }
    }
}