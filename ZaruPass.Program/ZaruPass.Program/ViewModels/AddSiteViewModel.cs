using MsBox.Avalonia;
using PasswordGenerator;
using ZaruPass.Core.Crypt;
using ZaruPass.Core.DataBase;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using ZaruPass.Core.Configuration;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ZaruPass.Program.ViewModels;

public partial class AddSiteViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _domain;
    private readonly Configurator _configurator;
    private readonly PasswordRepository _repository;
    public AddSiteViewModel(PasswordRepository repository, Configurator configurator)
    {
        _repository = repository;
        _configurator = configurator;
    }
    [RelayCommand]
    public async Task Ok()
    {
        if (string.IsNullOrEmpty(_domain))
        {
            await MessageBoxManager.GetMessageBoxStandard("Error", "Пустой домен", MsBox.Avalonia.Enums.ButtonEnum.Ok).ShowAsync();
            return;
        }
        string password = new Password(12).Next();

        byte[] passwordBytes = Encoder.Encode(password, _configurator.GetSecretKey());
        _repository.AddPassword(_domain, passwordBytes);

        await MessageBoxManager.GetMessageBoxStandard("Information", "Пароль был успешно добавлен", MsBox.Avalonia.Enums.ButtonEnum.Ok).ShowAsync();
    }
}
