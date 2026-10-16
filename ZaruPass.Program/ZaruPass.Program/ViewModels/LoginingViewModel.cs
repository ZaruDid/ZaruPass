using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using System;
using System.Threading.Tasks;
namespace ZaruPass.Program.ViewModels;

partial class LoginingViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _pinCode;
    [ObservableProperty]
    private string _pinCodeStars;

    public event Action<string> Event;

    [RelayCommand]
    public void AddSymbol(string value)
    {
        PinCode += value;
        PinCodeStars = new string('*', _pinCode.Length);
    }
    [RelayCommand]
    public void RemoveSymbol()
    {
        if (PinCode.Length > 0)
        {
            PinCode = PinCode.Substring(0, PinCode.Length - 1);
            PinCodeStars = new string('*', _pinCode.Length);
        }
    }
    [RelayCommand]
    public void ClearPinCode()
    {
        PinCode = string.Empty;
        PinCodeStars = string.Empty;
    }
    [RelayCommand]
    public async Task Forward()
    {
        if (string.IsNullOrEmpty(_pinCode) || _pinCode.Length < 4)
        {
            await MessageBoxManager.GetMessageBoxStandard("Error", "Длина пинкода должна быть больше 4-х символов", ButtonEnum.Ok).ShowAsync();
            return;
        }
        Event?.Invoke(_pinCode);
    }
}
