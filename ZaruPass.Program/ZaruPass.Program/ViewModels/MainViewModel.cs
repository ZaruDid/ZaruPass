using System;
using Avalonia.Controls;
using ZaruPass.Core.DataBase;
using System.Threading.Tasks;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.Input;
using ZaruPass.Core.Configuration;
using ZaruPass.Program.Features.Clipboard;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ZaruPass.Program.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private const float _s = 0.05f;
    private const float _s2 = 0.4f;
    [ObservableProperty]
    private IEnumerable<string> _sites;
    [ObservableProperty]
    private GridLength _sideBarWigth;
    private readonly PasswordRepository _repository;
    private readonly IClipboardService _clipboard;
    private readonly Configurator _configurator;

    public MainViewModel(PasswordRepository repository, IClipboardService clipboard, Configurator configurator)
    {
        _repository = repository;
        _clipboard = clipboard;
        _configurator = configurator;

        SideBarWigth = new GridLength(_s, GridUnitType.Star);
        Sites = _repository.GetSites();
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
    public async Task SetToClipboard(string site)
    {
        byte[] passwordBytes = _repository.GetPasswordBySite(site) ?? throw new ArgumentException("nullable password");
        string password = Core.Crypt.Encoder.Decode(passwordBytes, _configurator.Config.ConfigKey);
        await _clipboard.SetText(password);
    }
}
