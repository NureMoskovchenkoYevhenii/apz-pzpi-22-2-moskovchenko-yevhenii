using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Застосовуємо авторизацію до всього контролера
public class SensorDataController : ControllerBase
{
    private readonly SensorDataService _sensorDataService;
    private readonly SensorDataMapper _sensorDataMapper;
    private readonly IStringLocalizer<SharedResources> _localizer;

    public SensorDataController(SensorDataService sensorDataService, SensorDataMapper sensorDataMapper, IStringLocalizer<SharedResources> localizer)
    {
        _sensorDataService = sensorDataService;
        _sensorDataMapper = sensorDataMapper;
        _localizer = localizer;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllSensorData()
    {
        var sensorData = await _sensorDataService.GetAllSensorDataAsync();
        var sensorDataDtos = sensorData.Select(data => _sensorDataMapper.MapToDto(data));
        return Ok(sensorDataDtos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetSensorDataById(int id)
    {
        var sensorData = await _sensorDataService.GetSensorDataByIdAsync(id);
        if (sensorData == null)
        {
            // Повертаємо локалізовану помилку
            return NotFound(_localizer["SensorDataNotFound", id].Value);
        }
        var sensorDataDto = _sensorDataMapper.MapToDto(sensorData);
        return Ok(sensorDataDto);
    }
   
    [HttpPost]
    public async Task<IActionResult> AddSensorData([FromBody] SensorDataDto sensorDataDto)
    {
        if (sensorDataDto == null)
        {
            return BadRequest(_localizer["InvalidData"].Value);
        }

        var sensorData = _sensorDataMapper.MapToEntity(sensorDataDto);
        await _sensorDataService.AddSensorDataAsync(sensorData);
        sensorDataDto = _sensorDataMapper.MapToDto(sensorData);

        return CreatedAtAction(nameof(GetSensorDataById), new { id = sensorData.Id }, sensorDataDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSensorData(int id, [FromBody] SensorDataDto updatedSensorDataDto)
    {
        if (updatedSensorDataDto == null)
        {
            return BadRequest(_localizer["InvalidData"].Value);
        }
        var updatedSensorData = _sensorDataMapper.MapToEntity(updatedSensorDataDto);
        await _sensorDataService.UpdateSensorDataAsync(id, updatedSensorData);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSensorData(int id)
    {
        await _sensorDataService.DeleteSensorDataAsync(id);
        return NoContent();
    }
}