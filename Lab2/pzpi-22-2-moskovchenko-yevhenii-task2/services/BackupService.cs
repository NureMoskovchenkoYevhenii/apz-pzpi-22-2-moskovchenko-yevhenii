using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration; // Додайте цей using

public class BackupService
{
    private readonly IConfiguration _configuration;

    public BackupService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<(bool Success, string Message)> CreateBackupAsync()
    {
        // Отримуємо облікові дані з конфігурації (з docker-compose.yml)
        var connectionString = _configuration.GetConnectionString("DefaultConnection");

        // Простий парсер рядка підключення. Для продакшну краще використовувати спеціальні бібліотеки.
        var settings = connectionString.Split(';')
            .Select(s => s.Split('='))
            .ToDictionary(a => a[0].Trim(), a => a[1].Trim());

        var host = settings["Host"];
        var port = settings["Port"];
        var database = settings["Database"];
        var username = settings["Username"];
        var password = settings["Password"];

        // Формуємо ім'я файлу для бекапу
        var backupFileName = $"backup_{DateTime.UtcNow:yyyyMMdd_HHmmss}.sql";
        var backupFilePath = $"/backups/{backupFileName}"; // Папка, яку ми змонтували в docker-compose

        // Формуємо команду pg_dump
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "pg_dump",
                Arguments = $"-h {host} -p {port} -U {username} -d {database} -f {backupFilePath}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            }
        };

        // Передаємо пароль через змінну середовища, щоб він не був видимий у списку процесів
        process.StartInfo.EnvironmentVariables["PGPASSWORD"] = password;

        process.Start();
        string error = await process.StandardError.ReadToEndAsync();
        await process.WaitForExitAsync();

        if (process.ExitCode == 0)
        {
            return (true, $"Backup created successfully: {backupFileName}");
        }
        else
        {
            // Повертаємо помилку, якщо щось пішло не так
            return (false, $"Error creating backup: {error}");
        }
    }
}