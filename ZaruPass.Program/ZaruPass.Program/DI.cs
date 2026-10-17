using System;
using Serilog;
using System.IO;
using ZaruPass.Core.DataBase;
using ZaruPass.Core.Configuration;
using ZaruPass.Program.ViewModels;
using ZaruPass.Program.Features.Clipboard;
using Microsoft.Extensions.DependencyInjection;

namespace ZaruPass.Program;

public static class DI  
{
    public static void AddProgramServices(this IServiceCollection services)
    {
        string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ZaruPass");
        var logger = new LoggerConfiguration()
            .WriteTo.File(Path.Combine(path, "logs/log-.txt"), rollingInterval: RollingInterval.Day)
            .CreateLogger();

        services.AddLogging(builder => builder.AddSerilog(logger));

        services.AddTransient<PasswordRepository>();
        services.AddSingleton(_=> new Configurator()
            .SetFolder(path)
            .Build());
        services.AddTransient<IClipboardService, SimpleClipboard>();

        services.AddTransient<MainViewModel>();
        services.AddTransient<AuthorizationWindowViewModel>();
        services.AddTransient<ConfigurationViewModel>();
        services.AddTransient<LoginingViewModel>();
    }
}
