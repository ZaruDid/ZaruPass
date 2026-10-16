using System.Threading.Tasks;

namespace ZaruPass.Program.Features.Clipboard;

public interface IClipboardService
{
    Task SetText(string text);
    Task<string> GetText();
    Task ClearText();
}
