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
    private IStorageFolder _directory;
    [ObservableProperty]
    private string _secretKey;
    [ObservableProperty]
    private string _secretKeyForConfig;
    [RelayCommand]
    public async void Configure()
    {
        if(_directory == null)
        {
            await MessageBoxManager.GetMessageBoxStandard("Error", "Выберите папку!", ButtonEnum.Ok).ShowAsync();
            return;
        }
        if(string.IsNullOrEmpty(_secretKey))
        {
            await MessageBoxManager.GetMessageBoxStandard("Error", "Введите ключ шифрования!", ButtonEnum.Ok).ShowAsync();
            return;
        }
        if(string.IsNullOrEmpty(_secretKeyForConfig))
        {
            await MessageBoxManager.GetMessageBoxStandard("Error", "Введите ключ шифрования конфига!", ButtonEnum.Ok).ShowAsync();
            return;
        }
        string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var configurator = new Configurator(new Config { Path = _directory.TryGetLocalPath() + "\\File", SecretKey = _secretKeyForConfig }, _secretKey, appDataPath, SaveBytes);
    }
    private async void SaveBytes(string path, byte[] text)
    {
        var file = await _directory.CreateFileAsync(path);
        using var stream = await file.OpenWriteAsync();

        await stream.WriteAsync(text);
    }
}
