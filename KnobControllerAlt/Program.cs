namespace KnobControllerAlt;

static class Program
{
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        Detection detection = new Detection();
        detection.Setup();

        Application.Run(new KnobControllerContext());
    }
}

public class KnobControllerContext : ApplicationContext
{
    private NotifyIcon trayIcon;

    public KnobControllerContext()
    {
        trayIcon = new NotifyIcon
        {
            Icon = new Icon("./AppIcon.ico", 512, 512),
            ContextMenuStrip = new ContextMenuStrip()
            {
                Items = { new ToolStripMenuItem("Exit", null, Exit) }
            },
            Visible = true,
            Text = "Knob Controller",
        };
    }

    void Exit(object? sender, EventArgs e)
    {
        trayIcon.Visible = false;
        Application.Exit();
    }
}

