using System.Text.Json.Serialization;

namespace ZaruPass.Core.Configuration;

public class Config
{
    [JsonIgnore]
    public string Path { get; set; }
    public byte[] PathBytes { get; set; }
    public string SecretKey { get; set; }
    [JsonIgnore]
    public string Pincode { get; set; }
    public byte[] PincodeBytes { get; set; }
}
