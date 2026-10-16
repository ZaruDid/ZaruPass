using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using System.Linq;
using ZaruPass.Program.ViewModels;

namespace ZaruPass.Program.Views;

public partial class ConfigurationView : UserControl
{
    public ConfigurationView()
    {
        InitializeComponent();
        //_context = new ConfigurationViewModel();
        //DataContext = _context;
    }

    private async void OpenFileButton_Clicked(object sender, RoutedEventArgs args)
    {
        if(DataContext is ConfigurationViewModel vm)
        {
            var topLevel = TopLevel.GetTopLevel(this);

            var files = await topLevel.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions());

            vm.Directory = files.FirstOrDefault();
        }
    }
}