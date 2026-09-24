using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using System.Linq;
using ZaruPass.Program.ViewModels;

namespace ZaruPass.Program.Views;

public partial class ConfigurationView : UserControl
{
    private readonly ConfigurationViewModel _context;
    public ConfigurationView()
    {
        InitializeComponent();
        _context = new ConfigurationViewModel();
        DataContext = _context;
    }

    private async void OpenFileButton_Clicked(object sender, RoutedEventArgs args)
    {
        var topLevel = TopLevel.GetTopLevel(this);

        var files = await topLevel.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions());

        _context.Directory = files.FirstOrDefault();
    }
}