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
        MessageBox.Show($"key {e.KeyboardData.VirtualCode}");

        if (e.KeyboardData.VirtualCode != GlobalKeyboardHook.VkSnapshot)
            return;

        // seems, not needed in the life.
        //if (e.KeyboardState == GlobalKeyboardHook.KeyboardState.SysKeyDown &&
        //    e.KeyboardData.Flags == GlobalKeyboardHook.LlkhfAltdown)
        //{
        //    MessageBox.Show("Alt + Print Screen");
        //    e.Handled = true;
        //}
        //else

        if (e.KeyboardState == GlobalKeyboardHook.KeyboardState.KeyDown)
        {
            MessageBox.Show("Print Screen");
            e.Handled = true;
        }
    }
}