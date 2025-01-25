namespace MidiCaptainConfigurator.Client;

public record MidiCaptainConfiguration()
{
    public GlobalConfiguration GlobalConfiguration { get; set; }
    public Page Page { get; set; }
    public string FileName { get; set; }
}

public record GlobalConfiguration
{
    public int LedBright { get; set; } = 30; // Default value
    public int ScreenBright { get; set; }
    public bool DarkFonts { get; set; }
    public string Wallpaper { get; set; }
    public double LongPressTiming { get; set; }
    public bool Wireless24G { get; set; }
    public int WirelessId { get; set; }
    public int WirelessDb { get; set; }
}

public record Page
{
    public string PageName { get; set; }
    public int Exp1Ch { get; set; }
    public int Exp1Cc { get; set; }
    public int Exp2Ch { get; set; }
    public int Exp2Cc { get; set; }
    public int EncoderCc { get; set; }
    public string EncoderName { get; set; }
    public bool MidiThrough { get; set; }
    public string DisplayNumberAbc { get; set; }
    public int GroupNumber { get; set; }
    public int DisplayPcOffset { get; set; }
    public int DisplayBankOffset { get; set; }
    public List<Key> Keys { get; set; } = new List<Key>();
}

public record Key
{
    public int KeyNumber { get; set; }
    public int KeyTimes { get; set; }
    public string LedMode { get; set; }
    public List<string> LedColor1 { get; set; } = new List<string>();
    public List<string> ShortDw1 { get; set; } = new List<string>();
    public List<string> LedColor2 { get; set; } = new List<string>();
    public List<string> ShortDw2 { get; set; } = new List<string>();
    public List<string> LedColor3 { get; set; } = new List<string>();
    public List<string> ShortDw3 { get; set; } = new List<string>();
    public List<string> ShortUp1 { get; set; } = new List<string>();
    public List<string> Long1 { get; set; } = new List<string>();
    public List<string> ShortUp2 { get; set; } = new List<string>();
    public List<string> Long2 { get; set; } = new List<string>();
    public List<string> ShortUp3 { get; set; } = new List<string>();
    public List<string> Long3 { get; set; } = new List<string>();
}