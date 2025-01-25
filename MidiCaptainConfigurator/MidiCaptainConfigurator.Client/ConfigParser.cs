using Microsoft.AspNetCore.Components.Forms;

namespace MidiCaptainConfigurator.Client;

public class ConfigParser
{
    public async Task<MidiCaptainConfiguration> ParseConfigurationFile(IBrowserFile browserFile)
    {
        using var reader = new StreamReader(browserFile.OpenReadStream());

        var lines = await GetLinesOfConfigFile(reader);
        var parsedConfig = ParseLines(lines);
        var configuration = PopulateConfiguration(parsedConfig);
        
        configuration.FileName = browserFile.Name;

        return configuration;
    }

    private MidiCaptainConfiguration PopulateConfiguration(
        Dictionary<string, Dictionary<string, List<string>>> parsedConfig)
    {
        var configuration = new MidiCaptainConfiguration();

        foreach (var section in parsedConfig)
        {
            switch (section.Key)
            {
                case "[globalsetup]":
                {
                    var globalConfig = new GlobalConfiguration
                    {
                        LedBright = int.Parse(section.Value["ledbright"][0]),
                        ScreenBright = int.Parse(section.Value["screenbright"][0]),
                        DarkFonts = section.Value["dark_fonts"][0] == "on",
                        Wallpaper = section.Value["wallpaper"][0],
                        LongPressTiming = double.Parse(section.Value["long_press_timing"][0]),
                        Wireless24G = section.Value["WIRELESS_2.4G"][0] == "on",
                        WirelessId = int.Parse(section.Value["WIRELESS_ID"][0]),
                        WirelessDb = int.Parse(section.Value["WIRELESS_dB"][0])
                    };
                    configuration.GlobalConfiguration = globalConfig;
                    break;
                }
                case "[PAGE]":
                {
                    var page = new Page
                    {
                        PageName = section.Value["page_name"][0],
                        Exp1Ch = int.Parse(section.Value["exp1_CH"][0]),
                        Exp1Cc = int.Parse(section.Value["exp1_CC"][0]),
                        Exp2Ch = int.Parse(section.Value["exp2_CH"][0]),
                        Exp2Cc = int.Parse(section.Value["exp2_CC"][0]),
                        EncoderCc = int.Parse(section.Value["encoder_CC"][0]),
                        EncoderName = section.Value["encoder_NAME"][0],
                        MidiThrough = section.Value["midithrough"][0] == "on",
                        DisplayNumberAbc = section.Value["display_number_ABC"][0],
                        GroupNumber = int.Parse(section.Value["group_number"][0]),
                        DisplayPcOffset = int.Parse(section.Value["display_pc_offset"][0]),
                        DisplayBankOffset = int.Parse(section.Value["display_bank_offset"][0])
                    };
                    configuration.Page = page;
                    break;
                }
                default:
                {
                    if (section.Key.StartsWith("[key"))
                    {
                        var keyNumber = int.Parse(section.Key.Substring(4, section.Key.Length - 5));
                        var key = new Key
                        {
                            KeyNumber = keyNumber,
                            KeyTimes = int.Parse(section.Value["keytimes"][0]),
                            LedMode = section.Value.ContainsKey("ledmode") ? section.Value["ledmode"][0] : null,
                            LedColor1 = section.Value.ContainsKey("ledcolor1") ? section.Value["ledcolor1"].ToList() : new List<string>(),
                            ShortDw1 = section.Value.ContainsKey("short_dw1") ? section.Value["short_dw1"].ToList() : new List<string>(),
                            LedColor2 = section.Value.ContainsKey("ledcolor2") ? section.Value["ledcolor2"].ToList() : new List<string>(),
                            ShortDw2 = section.Value.ContainsKey("short_dw2") ? section.Value["short_dw2"].ToList() : new List<string>(),
                            LedColor3 = section.Value.ContainsKey("ledcolor3") ? section.Value["ledcolor3"].ToList() : new List<string>(),
                            ShortDw3 = section.Value.ContainsKey("short_dw3") ? section.Value["short_dw3"].ToList() : new List<string>(),
                            ShortUp1 = section.Value.ContainsKey("short_up1") ? section.Value["short_up1"].ToList() : new List<string>(),
                            Long1 = section.Value.ContainsKey("long1") ? section.Value["long1"].ToList() : new List<string>(),
                            ShortUp2 = section.Value.ContainsKey("short_up2") ? section.Value["short_up2"].ToList() : new List<string>(),
                            Long2 = section.Value.ContainsKey("long2") ? section.Value["long2"].ToList() : new List<string>(),
                            ShortUp3 = section.Value.ContainsKey("short_up3") ? section.Value["short_up3"].ToList() : new List<string>(),
                            Long3 = section.Value.ContainsKey("long3") ? section.Value["long3"].ToList() : new List<string>()
                        };
                        configuration.Page.Keys.Add(key);
                    }

                    break;
                }
            }
        }

        return configuration;
    }

    private static async Task<List<string>> GetLinesOfConfigFile(StreamReader reader)
    {
        // Read the lines of the file into List<string>
        var lines = new List<string>();

        while (await reader.ReadLineAsync() is { } line)
        {
            lines.Add(line);
        }

        return lines;
    }

    public Dictionary<string, Dictionary<string, List<string>>> ParseLines(List<string> lines)
    {
        var result = new Dictionary<string, Dictionary<string, List<string>>>();
        string currentSection = null;

        foreach (var trimmedLine in lines
                     .Select(line => line.Trim())
                     .Where(trimmedLine => !trimmedLine.StartsWith("#")
                                           && !string.IsNullOrEmpty(trimmedLine)))
        {
            // Identify sections
            if (trimmedLine.StartsWith("[") && trimmedLine.EndsWith("]"))
            {
                currentSection = trimmedLine;
                if (!result.ContainsKey(currentSection))
                {
                    result[currentSection] = new Dictionary<string, List<string>>();
                }

                continue;
            }

            // Parse properties and values
            if (currentSection == null) continue;

            var parts = trimmedLine.Split(['='], 2);

            if (parts.Length != 2) continue;

            var property = parts[0].Trim();
            var values = parts[1].Trim().Trim('[', ']').Split(["]["], StringSplitOptions.None);
            if (!result[currentSection].ContainsKey(property))
            {
                result[currentSection][property] = [];
            }

            result[currentSection][property].AddRange(values);
        }

        return result;
    }
}