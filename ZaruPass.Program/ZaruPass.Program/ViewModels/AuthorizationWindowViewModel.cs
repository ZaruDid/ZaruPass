using CommunityToolkit.Mvvm.ComponentModel;

namespace ZaruPass.Program.ViewModels;

partial class AuthorizationWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private bool _isPinCodeVisible;
    private string _text;
    public AuthorizationWindowViewModel()
    {
        IsPinCodeVisible = false;
    }
}
