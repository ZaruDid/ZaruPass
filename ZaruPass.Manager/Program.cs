using PasswordGenerator;
using Spectre.Console;
using ZaruPass.Core.Configuration;
using ZaruPass.Core.Crypt;
using ZaruPass.Core.DataBase;

string SECKRET = "";
const string COMMAND_ADD_NEW_PASSWORD = "Добавить новый пароль";
const string COMMAND_EDIT_PASSWORD = "Перегенерировать пароль";
const string COMMAND_DELETE_PASSWORD = "Удалить пароль";
const string COMMAND_FIND_PASSWORD = "Найти пароль";
const string COMMAND_CONFIGURATE = "Провести конфигурацию";
const string COMMAND_EXIT = "Выход";
const int PASSWORD_LENGTH = 12;

var configurator = new Configurator();
if(!configurator.HasConfig() || !File.Exists(configurator.Config.Directory))
{
    Configure();
}
configurator = new Configurator();
var secretBytes = File.ReadAllBytes(configurator.Config.Directory);
SECKRET = Encoder.Decode(secretBytes, configurator.Config.SecretKey);

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

void DeletePassword()
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

    var passwordObject = repo.GetBySite(site);
    if (passwordObject is not null)
        repo.DeletePassword(passwordObject);
}

void EditPassword()
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

    var passwordObject = repo.GetBySite(site);
    if(passwordObject is not null ) 
        repo.DeletePassword(passwordObject);

    var generatedPassword = new Password(PASSWORD_LENGTH).Next();
    var password = Encoder.Encode(generatedPassword, SECKRET);

    repo.AddPassword(site, password);
}

void FindPassword()
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

    byte[] passwordBytes = repo.GetPasswordBySite(site);
    string password = Encoder.Decode(passwordBytes, SECKRET);
    AnsiConsole.MarkupLine($"[green]{site}[/] - [red]{password}[/]");
    Console.ReadKey();
}

void ConfigurePasswords(string newKey)
{
    var repo = new PasswordRepository();
    var oldPasswords = repo.GetPasswords();
    repo.ClearPasswords();
    foreach(var password in oldPasswords)
    {
        var passwordString = Encoder.Decode(password.Pass, SECKRET);
        repo.AddPassword(password.Site, Encoder.Encode(passwordString, newKey));
    }
}

void Configure()
{
    var directory = AnsiConsole.Ask<string>("Введите путь для хранения ключа");
    if (!Directory.Exists(directory)) 
        throw new Exception("Path not found");
    directory += "\\file";

    var configPassword = AnsiConsole.Ask<string>("Введите пароль для шифрации основного пароля");

    var configurator = new Configurator();
    configurator.SetConfigFile(new Config() { Directory = directory, SecretKey = configPassword });

    var password = AnsiConsole.Ask<string>("Введите пароль для шифрации");
    var passwordCode = Encoder.Encode(password, configurator.Config.SecretKey);

    if (configurator.HasConfig())
    {
        ConfigurePasswords(password);
        SECKRET = password;
    }

    File.WriteAllBytes(directory, passwordCode);
    Console.ReadKey();
}

void AddNewPassword()
{
    var site = AnsiConsole.Ask<string>("Введите домен сайта");

    var pwd = new Password(PASSWORD_LENGTH).Next();
    AnsiConsole.MarkupLine($"[green]{site}[/] - [red]{pwd}[/]");

    var password = Encoder.Encode(pwd, SECKRET);

    var repo = new PasswordRepository();
    repo.AddPassword(site, password);
    Console.ReadKey();
}