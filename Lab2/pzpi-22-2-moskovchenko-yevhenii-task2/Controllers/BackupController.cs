using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")] // Тільки адміністратор може робити бекапи
public class BackupController : ControllerBase
{
    private readonly BackupService _backupService;

    public BackupController(BackupService backupService)
    {
        _backupService = backupService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateBackup()
    {
        var (success, message) = await _backupService.CreateBackupAsync();
        if (success)
        {
            return Ok(new { Message = message });
        }
        else
        {
            // Повертаємо помилку сервера, якщо бекап не вдався
            return StatusCode(500, new { Error = message });
        }
    }
}