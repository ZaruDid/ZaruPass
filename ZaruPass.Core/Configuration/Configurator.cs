using System.IO;
using System.Text;
using System.Text.Json;

namespace ZaruPass.Core.Configuration;

public class Configurator
{
    private const string _configName = "config.json";

    public Config Config { get; private set; }
    public Configurator()
    {
        if(HasConfig())
        {
            var configText = File.ReadAllText(_configName);
            Config = JsonSerializer.Deserialize<Config>(configText) ?? new Config();
            Config.Directory = Crypt.Encoder.Decode(Config.DirectoryBytes, Config.SecretKey);
            Config.DirectoryBytes = new byte[0];
        }
        else
        {
            Config = new Config();
        }
    }

    public void SetConfigFile(Config config)
    {
        Config = config;
        config.DirectoryBytes = Crypt.Encoder.Encode(config.Directory, config.SecretKey);
        var configJsonString = JsonSerializer.Serialize(config, new JsonSerializerOptions() {WriteIndented = true});
        File.WriteAllText(_configName, configJsonString);
    }

    public bool HasConfig()
    {
        return File.Exists(_configName);
    }
}
