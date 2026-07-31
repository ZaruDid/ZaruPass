using System.Text.Json.Serialization;

namespace ZaruPass.Core.Configuration;

public class Config
{
    [JsonIgnore]
    public string Directory { get; set; }
    public byte[] DirectoryBytes { get; set; }
    public string SecretKey { get; set; }
}
