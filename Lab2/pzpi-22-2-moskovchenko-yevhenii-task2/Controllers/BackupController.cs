using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization; 

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")] // Тільки адміністратор може робити бекапи
public class BackupController : ControllerBase
{
    private readonly BackupService _backupService;
    private readonly IStringLocalizer<SharedResources> _localizer; // 1. Оголошуємо поле

    public BackupController(
        BackupService backupService,
        IStringLocalizer<SharedResources> localizer) // Впроваджуємо залежність
    {
        _backupService = backupService;
        _localizer = localizer; // 2. Ініціалізуємо поле в конструкторі
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateBackup()
    {
        var (success, backupFileName) = await _backupService.CreateBackupAsync();
        if (success)
        {
            // Тепер _localizer існує і може бути використаний
            return Ok(new { Message = _localizer["BackupCreated"].Value, BackupFile = backupFileName });
        }
        else
        {
            return StatusCode(500, new { Error = backupFileName });
        }
    }

    [HttpGet("list")]
    public IActionResult ListBackups()
    {
        var backups = _backupService.GetAvailableBackups();
        return Ok(backups);
    }


    // POST /api/Backup/restore
    [HttpPost("restore")]
    public async Task<IActionResult> RestoreBackup([FromQuery] string fileName)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            return BadRequest("Backup file name must be provided.");
        }

        var (success, message) = await _backupService.RestoreBackupAsync(fileName);
        if (success)
        {
            return Ok(new { Message = message });
        }
        else
        {
            // Повертаємо помилку сервера, якщо відновлення не вдалося
            return StatusCode(500, new { Error = message });
        }
    }
}