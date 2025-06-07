using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class SensorDataController : ControllerBase
{
    private readonly SensorDataService _sensorDataService;
    private readonly SensorDataMapper _sensorDataMapper;

    public SensorDataController(SensorDataService sensorDataService, SensorDataMapper sensorDataMapper)
    {
        _sensorDataService = sensorDataService;
        _sensorDataMapper = sensorDataMapper;
    }

    [HttpGet]
    [Authorize]
    public IActionResult GetAllSensorData()
    {
        var sensorData = _sensorDataService.GetAllSensorData();
        var sensorDataDtos = sensorData.Select(data => _sensorDataMapper.MapToDto(data));
        return Ok(sensorDataDtos);
    }

    [HttpGet("{id}")]
    [Authorize]
    public IActionResult GetSensorDataById(int id)
    {
        var sensorData = _sensorDataService.GetSensorDataById(id);
        if (sensorData == null)
        {
            return NotFound();
        }
        var sensorDataDto = _sensorDataMapper.MapToDto(sensorData);
        return Ok(sensorDataDto);
    }

   
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddSensorData([FromBody] SensorDataDto sensorDataDto)
    {
        if (sensorDataDto == null)
        {
            return BadRequest("Дані відсутні!");
        }

        var sensorData = _sensorDataMapper.MapToEntity(sensorDataDto);
        _sensorDataService.AddSensorData(sensorData);
        sensorDataDto = _sensorDataMapper.MapToDto(sensorData);

        return CreatedAtAction(nameof(GetSensorDataById), new { id = sensorData.Id }, sensorDataDto);
    }



    [HttpPut("{id}")]
    [Authorize]
    public IActionResult UpdateSensorData(int id, SensorDataDto updatedSensorDataDto)
    {
        var updatedSensorData = _sensorDataMapper.MapToEntity(updatedSensorDataDto);
        _sensorDataService.UpdateSensorData(id, updatedSensorData);
        return NoContent();
    }


    [HttpDelete("{id}")]
    [Authorize]
    public IActionResult DeleteSensorData(int id)
    {
        _sensorDataService.DeleteSensorData(id);
        return NoContent();
    }

}
