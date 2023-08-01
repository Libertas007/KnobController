using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using WindowsInput;
using WindowsInput.Native;
using System.IO.Ports;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace KnobControllerAlt;

public class Controller
{
    static SerialPort port = new SerialPort();
        private static readonly HttpClient client = new HttpClient();
        
        public static void Initialize()
        {
            if (SerialPort.GetPortNames().Length == 0)
            {
                Environment.Exit(1);
            }
            
            string portName = SerialPort.GetPortNames()[0];


            port.PortName = portName;
            port.BaudRate = 115200;
            port.Handshake = Handshake.None;

            Config.Initialize();
            Setup();
        }

        static void NightMode()
        {
            AllOff();
            ChangeBar(7, Color.OFF, Effects.LIGHT);
            client.GetAsync($"http://{Config.lightIp}/night");
        }

        static void Setup()
        {
            AllWhite();
            ChangeLED(3, Color.BLUE, Effects.LIGHT);
            ChangeLED(6, Color.BLUE, Effects.LIGHT);
            ChangeLED(7, Color.RED, Effects.LIGHT);
            ChangeBar(7, Color.OFF, Effects.LIGHT);
            
            /*ChangeLED(2, Color.YELLOW, Effects.KEY_PRESS);
            ChangeLED(3, Color.WHITE, Effects.KEY_PRESS);
            ChangeLED(6, Color.WHITE, Effects.KEY_PRESS);
            ChangeLED(7, Color.WHITE, Effects.KEY_PRESS);*/
        }

        public static void SendCommand(string command)
        {
            port.Open();
            port.WriteLine(command);
            port.Close();
        }

        public static void AllRed()
        {
            SendCommand("red");
        }
        
        public static void AllBlue()
        {
            SendCommand("blue");
        }
        
        public static void AllGreen()
        {
            SendCommand("green");
        }
        
        public static void AllWhite()
        {
            SendCommand("white");
        }
        
        public static void AllOff()
        {
            SendCommand("off");
        }

        public static void ChangeBar(int length, Color color, int effect)
        {
            SendCommand($"bar {length} {color.red} {color.green} {color.blue} {effect}");            
        }
        
        public static void ChangeLED(int ledId, Color color, int effect)
        {
            SendCommand($"LED {ledId} {color.red} {color.green} {color.blue} {effect}");            
        }

        public static void SetBrightness(int brightness)
        {
            SendCommand($"brightness {brightness}");
        }
}

public class Color
{
    public int red;
    public int blue;
    public int green;

    public Color(int red, int green, int blue)
    {
        this.red = red;
        this.blue = blue;
        this.green = green;
    }

    public static Color RED = new Color(255, 0, 0);
    public static Color WHITE = new Color(255, 255, 255);
    public static Color BLUE = new Color(0, 0, 255);
    public static Color GREEN = new Color(0, 255, 0);
    public static Color YELLOW = new Color(255, 255, 0);
    public static Color OFF = new Color(0, 0, 0);
}

public class Effects
{
    public static int LIGHT = 0;
    public static int BLINK = 1;
    public static int RAINBOW = 4;
    public static int KEY_PRESS = 128;
}

public class Config
{
    public static readonly string configFile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\.knobcontroller\\config.yaml";
    private static ConfigFileStructure data;
    private static bool hasChanged = false;
    public static readonly string defaultConfig = @"
meta: false
";

    public static string lightIp = "192.168.44.34";
    public static bool meta;

    public static void Initialize()
    {
        if (!File.Exists(configFile))
        {
            File.WriteAllText(configFile, defaultConfig);
        }
        
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        string input = File.ReadAllText(configFile);

        data = deserializer.Deserialize<ConfigFileStructure>(input);

        meta = data.meta;
    }

    public static void SaveData()
    {
        if (hasChanged)
        {
            var serializer = new SerializerBuilder().Build();
            var yaml = serializer.Serialize(data);
            File.WriteAllText(configFile, yaml);
            hasChanged = false;
        }
    }

    public static bool IsMetaPressed()
    {
        return data.meta;
    }

    public static void SetMeta(bool value)
    {
        hasChanged = true;
        data.meta = value;
        meta = true;
    }
}

public class ConfigFileStructure
{
    public bool meta;
}

