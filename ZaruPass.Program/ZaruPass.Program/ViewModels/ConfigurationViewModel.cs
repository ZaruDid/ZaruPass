using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using System;
using System.IO;
using ZaruPass.Core.Configuration;

namespace ZaruPass.Program.ViewModels;

partial class ConfigurationViewModel : ViewModelBase
{
    [ObservableProperty]
    private ViewModelBase _page;
    [ObservableProperty]
    private bool _isVisiable = true;
    [ObservableProperty]
    private IStorageFolder _directory;
    [ObservableProperty]
    private string _secretKey;
    [ObservableProperty]
    private string _secretKeyForConfig;
    private readonly IServiceProvider _serviceProvider;
    private readonly Configurator _configurator;

    public ConfigurationViewModel(IServiceProvider serviceProvider, Configurator configurator)
    {
        _serviceProvider = serviceProvider;
        _configurator = configurator;
    }


    public async void Configure(string pincode)
    {
        _configurator.SetSaveFunction(SaveBytes)
            .SetSecretkey(_secretKey)
            .SetConfigFile(new Config{
                Path = Path.Combine(_directory.Path.LocalPath,"File"),
                ConfigKey = _secretKeyForConfig,
                Pincode = pincode
            });
        _configurator.Build();


        await MessageBoxManager.GetMessageBoxStandard("Success", "Успех", ButtonEnum.Ok).ShowAsync();
    }
    [RelayCommand]
    public async void Forward()
    {
        if (_directory == null)
        {
            await MessageBoxManager.GetMessageBoxStandard("Error", "Выберите папку!", ButtonEnum.Ok).ShowAsync();
            return;
        }
        if (string.IsNullOrEmpty(_secretKey))
        {
            await MessageBoxManager.GetMessageBoxStandard("Error", "Введите ключ шифрования!", ButtonEnum.Ok).ShowAsync();
            return;
        }
        if (string.IsNullOrEmpty(_secretKeyForConfig))
        {
            await MessageBoxManager.GetMessageBoxStandard("Error", "Введите ключ шифрования конфига!", ButtonEnum.Ok).ShowAsync();
            return;
        }
        IsVisiable = false;

        var p = _serviceProvider.GetRequiredService<LoginingViewModel>();
        p.Event += Configure;
        Page = p;
    }
    private async void SaveBytes(string path, byte[] text)
    {
        string name = Path.GetFileName(path);

        if (_directory is null) throw new ArgumentNullException("_directory can't be null"); 
        var file = await _directory.CreateFileAsync(name);
        using var stream = await file.OpenWriteAsync();

        await stream.WriteAsync(text);
    }
}
