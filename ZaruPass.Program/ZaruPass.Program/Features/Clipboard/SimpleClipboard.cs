using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input.Platform;
using Avalonia.VisualTree;
using System.Threading.Tasks;

namespace ZaruPass.Program.Features.Clipboard;

public class SimpleClipboard : IClipboardService
{
    public IClipboard? GetClipBoard()
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            return desktop.MainWindow?.Clipboard;
        }

        if (Application.Current?.ApplicationLifetime is ISingleViewApplicationLifetime singleView)
        {
            var mainView = singleView.MainView;
            if (mainView != null)
            {
                var visualRoot = mainView.GetVisualRoot();
                if (visualRoot is TopLevel topLevel)
                {
                    return topLevel.Clipboard;
                }
            }
        }
        return null;
    }

    public async Task ClearText()
    {
        if(GetClipBoard() is not null)
            await GetClipBoard().ClearAsync();
    }

    public async Task<string> GetText()
    {
        if (GetClipBoard() is not null)
            return await GetClipBoard().GetTextAsync();
        return "";
    }

    public async Task SetText(string text)
    {
        if (GetClipBoard() is not null)
            await GetClipBoard().SetTextAsync(text);
    }
}
