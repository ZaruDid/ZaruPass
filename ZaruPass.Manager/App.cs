using PasswordGenerator;
using Spectre.Console;
using ZaruPass.Core.Configuration;
using ZaruPass.Core.Crypt;
using ZaruPass.Core.DataBase;

namespace ZaruPass.Manager;

public class App
{
    private string _secret;
    private const int _passwordLength = 12;
    private Configurator _configurator;

    public App()
    {
        _configurator = new Configurator();
    }

    public void Start()
    {
        if (!_configurator.HasConfig() || !File.Exists(_configurator.Config.Path))
        {
            Configure();
        }
        var secretBytes = File.ReadAllBytes(_configurator.Config.Path);
        _secret = Encoder.Decode(secretBytes, _configurator.Config.SecretKey);

        MainWindow();
    }

    private void MainWindow()
    {
        const string COMMAND_ADD_NEW_PASSWORD = "Добавить новый пароль";
        const string COMMAND_EDIT_PASSWORD = "Перегенерировать пароль";
        const string COMMAND_DELETE_PASSWORD = "Удалить пароль";
        const string COMMAND_FIND_PASSWORD = "Найти пароль";
        const string COMMAND_CONFIGURATE = "Провести конфигурацию";
        const string COMMAND_EXIT = "Выход";

        var command = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Действие:")
                .AddChoices(
                COMMAND_ADD_NEW_PASSWORD,
                COMMAND_EDIT_PASSWORD,
                COMMAND_DELETE_PASSWORD,
                COMMAND_FIND_PASSWORD,
                COMMAND_CONFIGURATE,
                COMMAND_EXIT));

        switch (command)
        {
            case (COMMAND_ADD_NEW_PASSWORD):
                AddNewPassword();
                break;
            case (COMMAND_EDIT_PASSWORD):
                EditPassword();
                break;
            case (COMMAND_DELETE_PASSWORD):
                DeletePassword();
                break;
            case (COMMAND_FIND_PASSWORD):
                FindPassword();
                break;
            case (COMMAND_CONFIGURATE):
                Configure();
                break;
            case (COMMAND_EXIT):
                Environment.Exit(0);
                break;
        }
    }

    private string SelectSite()
    {
        var repo = new PasswordRepository();
        var sites = repo.GetSites();

        var site = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .Title("Выберете сайт")
            .PageSize(10)
            .EnableSearch()
            .SearchPlaceholderText("Начните ввод для поиска...")
            .AddChoices(sites));

        return site;
    }

    private void DeletePassword()
    {
        var repo = new PasswordRepository();
        string site = SelectSite();

        var passwordObject = repo.GetBySite(site);
        if (passwordObject is not null)
            repo.DeletePassword(passwordObject);
    }

     private void EditPassword()
    {
        var repo = new PasswordRepository();

        string site = SelectSite();

        var passwordObject = repo.GetBySite(site);
        if (passwordObject is not null)
            repo.DeletePassword(passwordObject);

        var generatedPassword = new Password(_passwordLength).Next();
        var password = Encoder.Encode(generatedPassword, _secret);

        repo.AddPassword(site, password);
    }

    private void FindPassword()
    {
        var repo = new PasswordRepository();

        string site = SelectSite();

        byte[] passwordBytes = repo.GetPasswordBySite(site) ?? new byte[0];
        string password = Encoder.Decode(passwordBytes, _secret);
        AnsiConsole.MarkupLine($"[green]{site}[/] - [red]{password}[/]");
        Console.ReadKey();
    }

    private void ConfigurePasswords(string newKey)
    {
        var repo = new PasswordRepository();
        var oldPasswords = repo.GetPasswords();
        repo.ClearPasswords();
        foreach (var password in oldPasswords)
        {
            var passwordString = Encoder.Decode(password.Pass, _secret);
            repo.AddPassword(password.Site, Encoder.Encode(passwordString, newKey));
        }
    }

    private void Configure()
    {
        var directory = AnsiConsole.Ask<string>("Введите путь для хранения ключа");
        if (!Directory.Exists(directory))
            throw new Exception("Path not found");
        directory += "\\file";

        var configPassword = AnsiConsole.Ask<string>("Введите пароль для шифрации основного пароля");

        var password = AnsiConsole.Ask<string>("Введите пароль для шифрации");
        var passwordCode = Encoder.Encode(password, configPassword);

        if (_configurator.HasConfig())
        {
            ConfigurePasswords(password);
            _secret = password;
        }

        _configurator.SetConfigFile(new Config() { Path = directory, SecretKey = configPassword });

        File.WriteAllBytes(directory, passwordCode);
        Console.ReadKey();
    }

    private void AddNewPassword()
    {
        var site = AnsiConsole.Ask<string>("Введите домен сайта");

        var pwd = new Password(_passwordLength).Next();
        AnsiConsole.MarkupLine($"[green]{site}[/] - [red]{pwd}[/]");

        var password = Encoder.Encode(pwd, _secret);

        var repo = new PasswordRepository();
        repo.AddPassword(site, password);
        Console.ReadKey();
    }
}
