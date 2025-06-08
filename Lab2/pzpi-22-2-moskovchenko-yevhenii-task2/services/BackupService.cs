using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration; // Додайте цей using

public class BackupService
{
    private readonly IConfiguration _configuration;

    private readonly string _backupsDirectory = "/backups"; // Шлях до папки з бекапами всередині контейнера
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
        var backupFilePath = $"/backups/{backupFileName}";

        // --- ОНОВЛЕННЯ: Додаємо опції --clean та --if-exists до pg_dump ---
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "pg_dump",
                // Додано --clean та --if-exists
                Arguments = $"--clean --if-exists -h {host} -p {port} -U {username} -d {database} -f {backupFilePath}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            }
        };

        process.StartInfo.EnvironmentVariables["PGPASSWORD"] = password;

        process.Start();
        string error = await process.StandardError.ReadToEndAsync();
        await process.WaitForExitAsync();

        if (process.ExitCode == 0)
        {
            return (true, $"Clean backup created successfully: {backupFileName}");
        }
        else
        {
            return (false, $"Error creating backup: {error}");
        }
    }

    public IEnumerable<string> GetAvailableBackups()
    {
        if (!Directory.Exists(_backupsDirectory))
        {
            return Enumerable.Empty<string>(); // Повертаємо порожній список, якщо папки немає
        }

        return Directory.GetFiles(_backupsDirectory, "*.sql")
                        .Select(Path.GetFileName) // Повертаємо тільки імена файлів
                        .OrderByDescending(f => f); // Сортуємо від нових до старих
    }

    // --- НОВИЙ МЕТОД: Відновлення з бекапу ---
    public async Task<(bool Success, string Message)> RestoreBackupAsync(string backupFileName)
    {
        var backupFilePath = Path.Combine(_backupsDirectory, backupFileName);

        // Перевіряємо, чи існує файл бекапу
        if (!File.Exists(backupFilePath))
        {
            return (false, $"Backup file not found: {backupFileName}");
        }

        // Отримуємо облікові дані з конфігурації
        var connectionString = _configuration.GetConnectionString("DefaultConnection");
        var settings = connectionString.Split(';')
            .Select(s => s.Split('='))
            .ToDictionary(a => a[0].Trim(), a => a[1].Trim());

        var host = settings["Host"];
        var port = settings["Port"];
        var database = settings["Database"];
        var username = settings["Username"];
        var password = settings["Password"];

        // Формуємо команду psql для відновлення
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "psql",
                // Додано --single-transaction (або -1)
                Arguments = $"--single-transaction -h {host} -U {username} -d {database} -f {backupFilePath}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            }
        };

        process.StartInfo.EnvironmentVariables["PGPASSWORD"] = password;

        process.Start();
        string error = await process.StandardError.ReadToEndAsync();
        await process.WaitForExitAsync();

        if (process.ExitCode == 0)
        {
            return (true, $"Restore from {backupFileName} completed successfully.");
        }
        else
        {
            return (false, $"Error restoring from backup: {error}");
        }
    }
    
}