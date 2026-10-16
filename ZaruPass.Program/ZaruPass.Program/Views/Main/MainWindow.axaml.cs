using Avalonia.Controls;
using ZaruPass.Program.Features.Clipboard;

namespace ZaruPass.Program.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Global.Clipboard = new SimpleClipboard(this);
        }
    }
}