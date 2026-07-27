using System.IO;
using System.Text.Json;
using ZaruPass.Core.Crypt;

namespace ZaruPass.Core.Configuration;

public class Configurator
{
    public string SecretKey = "SUPERMEGATOPSICKRET";
    private const string _configName = "config";

    public Config Config;
    public Configurator()
    {
        if(HasConfig())
        {
            var configBytes = File.ReadAllBytes(_configName);
            var configString = Encoder.Decode(configBytes, SecretKey);
            Config = JsonSerializer.Deserialize<Config>(configString) ?? new Config();
        }
        else
        {
            Config = new Config();
        }
    }

    public void SetConfigFile(Config config)
    {
        var configJsonString = JsonSerializer.Serialize(config);
        var configBytes = Encoder.Encode(configJsonString, SecretKey);
        File.WriteAllBytes(_configName, configBytes);
    }

    public bool HasConfig()
    {
        return File.Exists(_configName);
    }
}
