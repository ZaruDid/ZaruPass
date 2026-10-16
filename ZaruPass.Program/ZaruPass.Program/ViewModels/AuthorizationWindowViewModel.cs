using CommunityToolkit.Mvvm.ComponentModel;
using MsBox.Avalonia;
using System;
using ZaruPass.Core.Configuration;

namespace ZaruPass.Program.ViewModels;

partial class AuthorizationWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private ViewModelBase _page;
    private readonly Configurator _configurator;
    public AuthorizationWindowViewModel()
    {
        string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        _configurator = new Configurator(appDataPath);
        if (_configurator.HasConfig())
        {
            var p = new LoginingViewModel();
            p.Event += Forward;
            _page = p;
        }
        else { _page = new ConfigurationViewModel(); }
    }

    private async void Forward(string pinCode)
    {
        if(pinCode != _configurator.Config.Pincode)
        {
            await MessageBoxManager.GetMessageBoxStandard("Error", "Не верный пароль", MsBox.Avalonia.Enums.ButtonEnum.Ok).ShowAsync();
            return;
        }


    }
}
