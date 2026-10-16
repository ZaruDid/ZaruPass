using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZaruPass.Core.Configuration;
using ZaruPass.Core.DataBase;

namespace ZaruPass.Program.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private const float _s = 0.05f;
    private const float _s2 = 0.4f;
    [ObservableProperty]
    private IEnumerable<string> _sites;
    [ObservableProperty]
    private GridLength _sideBarWigth;

    public MainViewModel()
    {
        SideBarWigth = new GridLength(_s, GridUnitType.Star);
        var repo = new PasswordRepository();
        Sites = repo.GetSites();
    }
    [RelayCommand]
    public void ToggleSideBar()
    {
        if (SideBarWigth.Value == _s)
        {
            SideBarWigth = new GridLength(_s2, GridUnitType.Star);
        }
        else
        {
            SideBarWigth = new GridLength(_s, GridUnitType.Star);
        }
    }
    [RelayCommand]
    public async Task SetToClipboard(string? site)
    {
        var repo = new PasswordRepository();
        byte[] passwordBytes = repo.GetPasswordBySite(site) ?? throw new ArgumentException("nullable password");

        string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var configurator = new Configurator(appDataPath);
        string password = Core.Crypt.Encoder.Decode(passwordBytes, configurator.Config.SecretKey);

        await Global.Clipboard?.SetText(password);
    }
}
