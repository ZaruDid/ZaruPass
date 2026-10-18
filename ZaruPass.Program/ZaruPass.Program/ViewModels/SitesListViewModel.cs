using Avalonia.Input.Platform;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MsBox.Avalonia;
using PasswordGenerator;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ZaruPass.Core.Configuration;
using ZaruPass.Core.Crypt;
using ZaruPass.Core.DataBase;
using ZaruPass.Program.Features.Clipboard;

namespace ZaruPass.Program.ViewModels
{
    public partial class SitesListViewModel : ViewModelBase
    {
        [ObservableProperty]
        private IEnumerable<string> _sites;
        private readonly Configurator _configurator;
        private readonly IClipboardService _clipboard;
        private readonly PasswordRepository _repository;

        public SitesListViewModel(PasswordRepository repository,
                                  IClipboardService clipboard,
                                  Configurator configurator)
        {
            _repository = repository;
            _clipboard = clipboard;
            _configurator = configurator;

            Sites = _repository.GetSites();
        }
        [RelayCommand]
        public async Task SetToClipboard(string site)
        {
            byte[] passwordBytes = _repository.GetPasswordBySite(site) ?? throw new ArgumentException("nullable password");
            string password = Core.Crypt.Encoder.Decode(passwordBytes, _configurator.GetSecretKey());
            await _clipboard.SetText(password);
        }
        [RelayCommand]
        public async Task DeletePassword(string site)
        {
            var result = await MessageBoxManager.GetMessageBoxStandard("Information", "Вы точно хотите удалить пароль?", MsBox.Avalonia.Enums.ButtonEnum.OkCancel).ShowAsync();
            if(result == MsBox.Avalonia.Enums.ButtonResult.Ok)
            {
                _repository.DeletePassword(site);
                await MessageBoxManager.GetMessageBoxStandard("Information", "Пароль был удалён", MsBox.Avalonia.Enums.ButtonEnum.Ok).ShowAsync();
                Sites = _repository.GetSites();
            }
        }
        [RelayCommand]
        public async Task ReGeneratePassword(string site)
        {
            var result = await MessageBoxManager.GetMessageBoxStandard("Information", "Вы точно хотите заного сгенерировать пароль?", MsBox.Avalonia.Enums.ButtonEnum.OkCancel).ShowAsync();
            if (result == MsBox.Avalonia.Enums.ButtonResult.Ok)
            {
                string password = new Password(12).Next();
                byte[] passwordBytes = Encoder.Encode(password, _configurator.GetSecretKey());
                _repository.UpdatePassword(site, passwordBytes);
                await MessageBoxManager.GetMessageBoxStandard("Information", "Пароль был сгенерирован", MsBox.Avalonia.Enums.ButtonEnum.Ok).ShowAsync();
                Sites = _repository.GetSites();
            }    
        }
    }
}
