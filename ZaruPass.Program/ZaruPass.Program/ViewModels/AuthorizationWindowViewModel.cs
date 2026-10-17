using System;
using MsBox.Avalonia;
using ZaruPass.Core.Configuration;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;

namespace ZaruPass.Program.ViewModels;

partial class AuthorizationWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private ViewModelBase _page;
    private readonly Configurator _configurator;
    private readonly IServiceProvider _serviceProvider;
    public AuthorizationWindowViewModel(IServiceProvider serviceProvider, Configurator configurator)
    {
        _serviceProvider = serviceProvider;
        _configurator = configurator;

        if (_configurator.HasConfig())
        {
            var p = _serviceProvider.GetRequiredService<LoginingViewModel>();
            p.Event += Forward;
            _page = p;
        }
        else { _page = _serviceProvider.GetRequiredService<ConfigurationViewModel>(); }
    }

    private async void Forward(string pinCode)
    {
        if(pinCode != _configurator.Config.Pincode)
        {
            await MessageBoxManager.GetMessageBoxStandard("Error", "Не верный пароль", MsBox.Avalonia.Enums.ButtonEnum.Ok).ShowAsync();
            return;
        }

        Page = _serviceProvider.GetRequiredService<MainViewModel>();
    }
}
