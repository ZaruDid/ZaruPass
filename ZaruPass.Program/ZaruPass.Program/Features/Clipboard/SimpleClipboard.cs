using Avalonia.Controls;
using System.Threading.Tasks;

namespace ZaruPass.Program.Features.Clipboard;

public class SimpleClipboard : IClipboardService
{
    public readonly TopLevel _topLevel;
    public SimpleClipboard(TopLevel topLevel)
    {
        _topLevel = topLevel;
    }

    public async Task ClearText()
    {
        await _topLevel.Clipboard?.ClearAsync();
    }

    public async Task<string> GetText()
    {
        return await _topLevel.Clipboard?.GetTextAsync();    
    }

    public async Task SetText(string text)
    {
        await _topLevel.Clipboard?.SetTextAsync(text);
    }
}
