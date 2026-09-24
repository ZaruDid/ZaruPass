using Avalonia.Controls;
using ZaruPass.Program.ViewModels;

namespace ZaruPass.Program.Views;

public partial class LoginingView : UserControl
{
    public LoginingView()
    {
        InitializeComponent();
        DataContext = new LoginingViewModel();
    }
}