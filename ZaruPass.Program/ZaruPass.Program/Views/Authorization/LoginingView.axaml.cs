using Avalonia.Controls;
using ZaruPass.Program.ViewModels;

namespace ZaruPass.Program.Views;

public partial class LoginingView : UserControl
{
    public LoginingView()
    {
        InitializeComponent();
        //if(DataContext is null) DataContext = new LoginingViewModel();
    }
}