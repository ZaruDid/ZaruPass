using Avalonia.Controls;
using ZaruPass.Program.ViewModels;

namespace ZaruPass.Program.Views;

public partial class AuthorizationView : UserControl
{
    public AuthorizationView()
    {
        InitializeComponent();
        DataContext = new AuthorizationWindowViewModel();
    }
}