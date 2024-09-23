namespace KnobControllerAlt;
using Microsoft.VisualBasic;

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
        
        Controller.Initialize();

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
                Items = { 
                    new ToolStripMenuItem("Terminal", null, Terminal),
                    new ToolStripMenuItem("Connect", null, Connect),
                    new ToolStripMenuItem("Exit", null, Exit),
                }
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
    
    void Connect(object? sender, EventArgs e)
    {
        Controller.Initialize();
    }

    void Terminal(object? sender, EventArgs e)
    {
        string command = Interaction.InputBox("Enter command: ", "Terminal");

        Controller.SendCommand(command);
    }
}

