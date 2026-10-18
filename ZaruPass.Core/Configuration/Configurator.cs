using System.Text;
using System.Text.Json;
using ZaruPass.Core.Crypt;
using ZaruPass.Core.DataBase.Migrations;

namespace ZaruPass.Core.Configuration;

public class Configurator
{
    private string _folder = "";
    private string _secretKey = "";
    private const string _configName = "config.json";
    public Config Config { get; private set; } = new Config();
    private Action<string, byte[]> _saveBytesFunc = File.WriteAllBytes;

    //public Configurator(Config config, string password, string folder = "", Action<string, byte[]> saveBytesFunc = null)
    //{
    //    _folder = Path.Combine(folder, _configFolderName);
    //    if (saveBytesFunc is null)
    //    {
    //        _saveBytesFunc = File.WriteAllBytes;
    //    }
    //    else
    //    {
    //        _saveBytesFunc = saveBytesFunc;
    //    }

    //    if (string.IsNullOrEmpty(config.ConfigKey)) throw new ArgumentException("ConfigKey cannot be null or empty");
    //    if(string.IsNullOrEmpty(config.Path)) throw new ArgumentException("Path cannot be null or empty");

    //    SetConfigFile(config);
    //    SaveSecrectKey(password, config.ConfigKey, config.Path);

    //}
    //public Configurator(string folder = "")
    //{
    //    _folder = Path.Combine(folder, _configFolderName);
    //    _saveBytesFunc = File.WriteAllBytes;

    //    if (HasConfig())
    //    {
    //        var configText = File.ReadAllText(Path.Combine(_folder, _configName));
    //        Config = JsonSerializer.Deserialize<Config>(configText) ?? new Config();

    //        Config.Path = Crypt.Encoder.Decode(Config.PathBytes, Config.ConfigKey);
    //        Config.PathBytes = new byte[0];

    //        if (Config.PincodeBytes != null)
    //        {
    //            Config.Pincode = Crypt.Encoder.Decode(Config.PincodeBytes, Config.ConfigKey);
    //            Config.PincodeBytes = new byte[0];
    //        }
    //    }
    //    else
    //    {
    //        Config = new Config();
    //    }
    //}
    public Configurator Build()
    {
        if (HasConfig())
        {
            var configText = File.ReadAllText(Path.Combine(_folder, _configName));
            Config = JsonSerializer.Deserialize<Config>(configText) ?? new Config();

            Config.Path = Crypt.Encoder.Decode(Config.PathBytes, Config.ConfigKey);
            Config.PathBytes = new byte[0];

            if (Config.PincodeBytes != null)
            {
                Config.Pincode = Crypt.Encoder.Decode(Config.PincodeBytes, Config.ConfigKey);
                Config.PincodeBytes = new byte[0];
            }
        }
        else if(!string.IsNullOrEmpty(_secretKey) && !string.IsNullOrEmpty(Config.ConfigKey) && !string.IsNullOrEmpty(Config.Path))
        {
            SaveConfig();
            SaveSecrectKey();
        }
        return this;
    }
    public Configurator SetSaveFunction(Action<string, byte[]> saveBytesFunc)
    {
        _saveBytesFunc = saveBytesFunc;
        return this;
    }
    public Configurator SetConfigFile(Config config)
    {
        Config = config;
        return this;
    }
    public Configurator SetFolder(string folder)
    {
        _folder = folder;
        return this;
    }
    public Configurator SetSecretkey(string secretKey)
    {
        _secretKey = secretKey;
        return this;
    }
    public bool HasConfig()
    {
        string filePath = Path.Combine(_folder, _configName);
        return File.Exists(filePath);
    }
    public string GetSecretKey()
    {
        string key = Crypt.Encoder.Decode(File.ReadAllBytes(Config.Path), Config.ConfigKey); 
        return key;
    }
    private void SaveConfig()
    {
        Config.PathBytes = Crypt.Encoder.Encode(Config.Path, Config.ConfigKey);
        if (Config.Pincode is not null) Config.PincodeBytes = Crypt.Encoder.Encode(Config.Pincode, Config.ConfigKey);
        var configJsonString = JsonSerializer.Serialize(Config, new JsonSerializerOptions() { WriteIndented = true });

        Directory.CreateDirectory(Path.Combine(_folder));
        string filePath = Path.Combine(_folder, _configName);

        File.WriteAllBytes(filePath, Encoding.UTF8.GetBytes(configJsonString));
    }
    private void SaveSecrectKey()
    {
        byte[] passwordBytes = Crypt.Encoder.Encode(_secretKey, Config.ConfigKey);
        _saveBytesFunc(Config.Path, passwordBytes);
    }
}
