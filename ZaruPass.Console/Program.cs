using SharpHook;
using System.Text.RegularExpressions;
using WindowsInput;
using ZaruPass.Core;
using SharpHook.Data;
using ZaruPass.Core.DataBase;
using ZaruPass.Core.Crypt;
using ZaruPass.Core.Configuration;

var configurator = new Configurator();
byte[] passwordBytes = File.ReadAllBytes(configurator.Config.Directory);
string SECKRET = Encoder.Decode(passwordBytes, configurator.Config.SecretKey);

var hook = new TaskPoolGlobalHook();
hook.KeyPressed += (s, e) =>
{
    if (e.Data.KeyCode == KeyCode.VcEnter)
    {
        string url = WindowHelper.GetBrowserUrl();
        Regex regex = new Regex(@"\/.+");
        string site = regex.Replace(url, "");
        Console.WriteLine($"Browser URL: {site}");

        var repository = new PasswordRepository();
        var passwordBytes = repository.GetPasswordBySite(site);
        var password = Encoder.Decode(passwordBytes, SECKRET);

        InputSimulator simulator = new InputSimulator();

        simulator.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.BACK);
        simulator.Keyboard.TextEntry(password);
        Environment.Exit(0);
    }
};

hook.RunAsync();

await Task.Delay(10000);