using System.Text;
using System.Text.Json;

namespace ZaruPass.Core.Configuration;

public class Configurator
{
    private const string _configName = "config.json";
    private const string _configFolderName = "ZaruPass";
    private readonly string _folder;
    private readonly Action<string, byte[]> _saveBytesFunc;

    public Config Config { get; private set; }
    public Configurator(Config config, string password, string folder = "", Action<string, byte[]> saveBytesFunc = null)
    {
        _folder = Path.Combine(folder, _configFolderName);
        if (saveBytesFunc is null)
        {
            _saveBytesFunc = File.WriteAllBytes;
        }
        else
        {
            _saveBytesFunc = saveBytesFunc;
        }

        if (string.IsNullOrEmpty(config.SecretKey)) throw new ArgumentException("SecretKey cannot be null or empty");
        if(string.IsNullOrEmpty(config.Path)) throw new ArgumentException("Path cannot be null or empty");

        SetConfigFile(config);

        var passwordBytes = Crypt.Encoder.Encode(password, config.SecretKey);

        _saveBytesFunc(config.Path, passwordBytes);

    }
    public Configurator(string folder = "",Action<string, byte[]> saveBytesFunc = null)
    {
        _folder = Path.Combine(folder, _configFolderName);
        if (saveBytesFunc is null)
        {
            _saveBytesFunc = File.WriteAllBytes;
        }
        else
        {
            _saveBytesFunc = saveBytesFunc;
        }

        if (HasConfig())
        {
            var configText = File.ReadAllText(Path.Combine(_folder, _configName));
            Config = JsonSerializer.Deserialize<Config>(configText) ?? new Config();
            
            Config.Path = Crypt.Encoder.Decode(Config.PathBytes, Config.SecretKey);
            Config.PathBytes = new byte[0];

            if(Config.PincodeBytes != null)
            {
                Config.Pincode = Convert.ToInt32(Crypt.Encoder.Decode(Config.PincodeBytes, Config.SecretKey));
                Config.PincodeBytes = new byte[0];
            }
        }
        else
        {
            Config = new Config();
        }
    }

    public void SetConfigFile(Config config)
    {
        Config = config;
        config.PathBytes = Crypt.Encoder.Encode(config.Path, config.SecretKey);
        var configJsonString = JsonSerializer.Serialize(config, new JsonSerializerOptions() {WriteIndented = true});

        Directory.CreateDirectory(Path.Combine(_folder));
        string filePath = Path.Combine(_folder, _configName);

        File.WriteAllBytes(filePath, Encoding.UTF8.GetBytes(configJsonString));
    }

    public bool HasConfig()
    {
        string filePath = Path.Combine(_folder, _configName);

        return File.Exists(filePath);
    }
}
