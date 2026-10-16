using Avalonia.Controls;
using ZaruPass.Program.ViewModels;

namespace ZaruPass.Program.Views;

public partial class AuthorizationWindow : Window
{
    public AuthorizationWindow()
    {
        InitializeComponent();
        //DataContext = new AuthorizationWindowViewModel();
    }
}