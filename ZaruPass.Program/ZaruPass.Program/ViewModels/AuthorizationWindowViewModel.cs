using CommunityToolkit.Mvvm.ComponentModel;
using System;
using ZaruPass.Core.Configuration;

namespace ZaruPass.Program.ViewModels;

partial class AuthorizationWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private ViewModelBase _page;
    public AuthorizationWindowViewModel()
    {
        string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var configurator = new Configurator(appDataPath);
        if (configurator.HasConfig())
        {
            _page = new LoginingViewModel();
        }
        else { _page = new ConfigurationViewModel(); }
    }
}
