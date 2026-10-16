using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using System;
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

    public async void Configure(string pincode)
    {
        string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var configurator = new Configurator(new Config { 
            Path = _directory.TryGetLocalPath() + "\\File", 
            SecretKey = _secretKeyForConfig,
            Pincode = pincode
        }, 
        _secretKey, appDataPath, SaveBytes);

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

        var p = new LoginingViewModel();
        p.Event += Configure;
        Page = p;
    }
    private async void SaveBytes(string path, byte[] text)
    {
        var file = await _directory.CreateFileAsync(path);
        using var stream = await file.OpenWriteAsync();

        await stream.WriteAsync(text);
    }
}
